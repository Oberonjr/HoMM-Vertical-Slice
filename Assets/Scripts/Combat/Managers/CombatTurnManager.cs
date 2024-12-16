using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatTurnManager : MonoBehaviour
{
    public static CombatTurnManager Instance;
    
    public List<Unit> unitsInCombat = new List<Unit>(); //TODO: Make this initialize based on the armies going into combat
    public int currentTurnIndex = 0;
    
    [HideInInspector]public Unit currentUnit;
    [HideInInspector]public Unit nextUnit;
    [HideInInspector]public GameObject indicator;

    [SerializeField] private GameObject aggressorArmySpawnParent;
    [SerializeField] private GameObject defenderArmySpawnParent;
    //TODO: Move these to a combat UI manager
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private GameObject TileHighlightSprite;
    
    private CombatStateMachine stateMachine;
    private Army aggressorArmy;
    private Army defenderArmy;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        stateMachine = new CombatStateMachine();
        CombatEventBus<CombatStartEvent>.OnEvent += StartCombat;
        CombatEventBus<UnitTurnStartEvent>.OnEvent += StartUnitTurn;
        CombatEventBus<UnitTurnEndEvent>.OnEvent += EndTurn;
        CombatEventBus<UnitEndMovingEvent>.OnEvent += OnUnitArrival;
        CombatEventBus<AttackStartEvent>.OnEvent += ApplyDamage;
        CombatEventBus<MouseLeftClickEvent>.OnEvent += HandleInput;
        CombatEventBus<UnitKilledEvent>.OnEvent += HandleUnitDeath;
        StartCoroutine(MyUtils.LateStart(0.1f, () =>
        {
            //Combat Start happens here. Doing it late so all the other components of the scene have time to initialize and set themselves before starting to use them
            stateMachine.ChangeState(new CombatStartState(stateMachine));
            
        }));
        indicator = Instantiate(indicatorPrefab);
    }
    
    private void OnDisable()
    {
        CombatEventBus<CombatStartEvent>.OnEvent -= StartCombat;
        CombatEventBus<UnitTurnStartEvent>.OnEvent -= StartUnitTurn;
        CombatEventBus<UnitTurnEndEvent>.OnEvent -= EndTurn;
        CombatEventBus<UnitEndMovingEvent>.OnEvent -= OnUnitArrival;
        CombatEventBus<AttackStartEvent>.OnEvent -= ApplyDamage;
        CombatEventBus<MouseLeftClickEvent>.OnEvent -= HandleInput;
        CombatEventBus<UnitKilledEvent>.OnEvent -= HandleUnitDeath;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    void HandleInput(MouseLeftClickEvent mEvent)
    {
        if (stateMachine.currentState == new UnitIdleState(stateMachine, currentUnit)||
            !currentUnit.CanReachNode(mEvent.position) ||
            mEvent.position == currentUnit.currentNodePosition)
        {
            return;
        }
        Node clickedNode = mEvent.position;

        Unit targetUnit;
        if (clickedNode.stationedUnit != null && clickedNode.stationedUnit != currentUnit) //Clicked node has a unit
        {
            targetUnit = clickedNode.stationedUnit;
            targetUnit.currentNodePosition.IsWalkable = true;
            //Check if the unit is adjacent to determine course of action
            if (currentUnit.currentNodePosition.neighbours.ContainsKey(targetUnit.currentNodePosition))//Target unit is adjacent
            {
                QueueAttackAction(targetUnit, false);
            }
            else 
            {
                //Get the path to the target
                List<Node> pathToTarget = Pathfinding.Instance.FindPath(currentUnit.currentNodePosition.GridPosition, targetUnit.currentNodePosition.GridPosition, GridTracker.Instance.CombatGrid);
                //As the tile that the target is not walkable, we need the node that comes before it
                pathToTarget.Remove(pathToTarget.Last());
                //Set our correct target node for movement
                Node arrivalNode = pathToTarget.Last();
                //Start moving towards target, then queue an attack
                QueueMoveAction(arrivalNode, () => QueueAttackAction(targetUnit, true));
                
            }
        }
        else
        {
            QueueMoveAction(clickedNode, null);
        }
    }
    
    public void StartCombat(CombatStartEvent e = null)
    {
        //TODO: Once event takes in Hero stats, make this read e to initialize the list with the armies' creatures
        // Sort units by initiative.
        if(CombatBridgeManager.Instance == null) throw new Exception("CombatBridgeManager is null");
        aggressorArmy = CombatBridgeManager.Instance.AggressorArmy;
        defenderArmy = CombatBridgeManager.Instance.DefenderArmy;
        SpawnArmy(aggressorArmy, aggressorArmySpawnParent, false);
        SpawnArmy(defenderArmy, defenderArmySpawnParent, true);
        unitsInCombat = unitsInCombat.OrderByDescending(u => u.unitStats.initiative).ToList();
        currentTurnIndex = 0;
        currentUnit = unitsInCombat[0];
        
    }

    public void SpawnArmy(Army army, GameObject location, bool isDefender)
    {
        for (int i = 0; i < location.transform.childCount; i++)
        {
            Transform spawnLocation = location.transform.GetChild(i);
            //Debug.Log(spawnLocation.name);
            spawnLocation.transform.position = MyUtils.ClosestNode(spawnLocation.transform.position).GridPosition;
            if (army._units[i].unitPrefab == null) return;
            GameObject spawnUnit = Instantiate(army._units[i].unitPrefab, spawnLocation.position, Quaternion.identity, spawnLocation.transform);
            spawnUnit.transform.parent = null;
            Unit unitComponent = spawnUnit.GetComponent<Unit>();
            unitComponent.stackSize = army._units[i].amount;
            if (army.owner != null)
            {
                unitComponent.OwnerArmy = army;
            }
            MyUtils.SnapToGridCenter(spawnUnit.transform, out unitComponent.currentNodePosition);
            unitsInCombat.Add(spawnUnit.GetComponent<Unit>());
            spawnUnit.GetComponentInChildren<SpriteRenderer>().flipX = isDefender;
            
        }
    }
    
    public void StartUnitTurn(UnitTurnStartEvent e = null)
    {
        foreach (Unit unit in unitsInCombat)
        {
            //Can't walk and target allies, but can walk and to enemies and target them. This might make enemies walkable through, but eh
            if (unit.OwnerArmy == currentUnit.OwnerArmy)
            {
                unit.currentNodePosition.IsWalkable = false;
            }
            else
            {
                unit.currentNodePosition.IsWalkable = true;
            }
            //Debug.Log(unit.name + "'s node is walkable? " + unit.currentNodePosition.IsWalkable);
        }
        
        //Debug.Log("Starting the turn of: " + currentUnit.name);
        currentUnit.currentNodePosition.IsWalkable = true;
        currentUnit.isUnitTurn = true;
        if (currentUnit.IsAI)
        {
            SkipTurn();
        }
        else
        {
            CombatUnitMovement.Instance.currentUnit = currentUnit;
        }
        
        currentUnit.ReplenishMovementPoints();
        indicator.transform.position = new Vector3(currentUnit.currentNodePosition.GridPosition.x, currentUnit.currentNodePosition.GridPosition.y + 1.25f, -1);
        indicator.transform.SetParent(currentUnit.transform);
        HighlightWalkableArea();
    }

    public void EndTurn(UnitTurnEndEvent e = null)
    {
        //Debug.Log("Ending the turn of: " + currentUnit.name);
        currentUnit.currentNodePosition.IsWalkable = false;
        currentUnit.isUnitTurn = false;
        currentTurnIndex++;
        if (currentTurnIndex >= unitsInCombat.Count)
        {
            Debug.Log("Starting new round");
            currentTurnIndex = 0;
        }
        nextUnit = GetNextUnit();
        if (nextUnit != null)
        {
            currentUnit = nextUnit;
        }
    }

    public void SkipTurn()
    {
        Debug.Log("Skipping the turn of: " + currentUnit.name);
        EndTurn();
    }

    public Unit GetNextUnit()
    {
        Unit pNextUnit = unitsInCombat[currentTurnIndex];
        return pNextUnit;
    }
    
    void QueueAttackAction(Unit targetUnit, bool queued)
    {
        currentUnit.QueuedAction = () =>
        {
            CombatEventBus<AttackStartEvent>.Publish(new AttackStartEvent(currentUnit, targetUnit));
        };
        stateMachine.ChangeState(new UnitAttackingState(stateMachine, currentUnit, targetUnit, queued));
    }

    void QueueMoveAction(Node targetNode, System.Action onArrival)
    {
        currentUnit.QueuedAction = onArrival;
        
        stateMachine.ChangeState(new UnitWalkingState(stateMachine, currentUnit, targetNode));
    }

    void OnUnitArrival(UnitEndMovingEvent e)
    {
        currentUnit.QueuedAction?.Invoke();
        
        currentUnit.QueuedAction = null;
        
        stateMachine.ChangeState(new UnitTurnEndState(stateMachine, currentUnit));
    }

    [HideInInspector] public List<GameObject> highlightedSprites = new List<GameObject>();
    void HighlightWalkableArea()
    {
        foreach (Node node in currentUnit.ReachableNodes())
        {
            GameObject nodeHighlight = Instantiate(TileHighlightSprite,
                new Vector3(node.GridPosition.x, node.GridPosition.y, 0), Quaternion.identity);
            highlightedSprites.Add(nodeHighlight);
            nodeHighlight.transform.SetParent(currentUnit.transform);
        }
    }

    public void ClearHighlightedSprites()
    {
        foreach (GameObject highlight in highlightedSprites)
        {
            Destroy(highlight);
        }
        highlightedSprites.Clear();
    }
    
    public int CalculateDamage(Unit attacker, Unit defender)
    {
        int baseDamage = attacker.CalculateDamage();
        int finalDamage = Mathf.Max(1, baseDamage + (attacker.unitStats.attack - defender.unitStats.defense));  // Basic formula.
        return finalDamage;
    }

    public IEnumerator AttackLogic(AttackStartEvent e)
    {
        Unit attacker = e.attacker;
        Unit defender = e.defender;
        int damage = CalculateDamage(attacker, defender);
        //Debug.Log(attacker.UnitName + " is attacking " + defender.UnitName + " for " + damage + " damage");
        defender.TakeDamage(damage);

        yield return new WaitForSeconds(0.7f);
        //TODO: Maybe handle retaliation logic someplace else...
        if (!defender.hasRetaliated && defender.stackSize > 0)
        {
            CombatEventBus<UnitRetaliateEvent>.Publish(new UnitRetaliateEvent(defender, attacker));
            attacker.TakeDamage(CalculateDamage(defender, attacker));
            defender.hasRetaliated = true;
        }
    }

    public void ApplyDamage(AttackStartEvent e)
    {
        StartCoroutine(AttackLogic(e));
    }

    void HandleUnitDeath(UnitKilledEvent e)
    {
        unitsInCombat.Remove(e.unit);
    }

    
}

