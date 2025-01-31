using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class Unit : MonoBehaviour
{
    public UnitStats unitStats;
    public int currentHP;
    public int currentMovementPoints;
    public int stackSize;
    public bool hasRetaliated;  
    public Node currentNodePosition;
    public bool isMoving;
    public bool IsAI;
    public Action QueuedAction;
    
    [HideInInspector]public string UnitName;
    [HideInInspector]public bool isUnitTurn;
    [HideInInspector]public Animator animator;
    [FormerlySerializedAs("OwnerHero")] [HideInInspector]public Army OwnerArmy; 

    private TMPro.TMP_Text stackSizeText;
    
    private void Start()
    {
            UnitName = unitStats.unitName;
            currentHP = unitStats.maxHP;
            currentMovementPoints = unitStats.movementSpeed;
            MyUtils.SnapToGridCenter(transform, out currentNodePosition);
            currentNodePosition.stationedUnit = this;
            animator = GetComponentInChildren<Animator>() ??
                       throw new System.Exception($"No animator component found on {name}'s VFX child");
            stackSizeText = transform.GetComponentInChildren<TMPro.TMP_Text>() ??
                            throw new System.Exception($"No text component found on {name}'s Canvas child");
            stackSizeText.text = stackSize.ToString();

    }
    

    public void TakeDamage(int damage)
    {
        int leftoverDamage = Mathf.Max(0, damage - currentHP);
        currentHP = Mathf.Max(0, currentHP - damage);
        if (currentHP == 0)
        {
            currentHP = unitStats.maxHP;
            stackSize--;
            OwnerArmy.RemoveUnit(unitStats, 1);
            stackSizeText.text = stackSize.ToString();
            if (stackSize == 0)
            {
                Die();
                return;
            }
        }
        if(leftoverDamage > 0 && stackSize > 0)
        {
            TakeDamage(leftoverDamage);
        }
        CombatEventBus<DamageReceivedEvent>.Publish(new DamageReceivedEvent(this));
        
    }

    public void Die()
    {
        // Handle unit death (remove from grid, remove from turn order, etc.)
        CombatEventBus<UnitKilledEvent>.Publish(new UnitKilledEvent(this));
        Debug.Log(name + " has been killed");
        //isAlive = false;
        
        currentNodePosition.IsWalkable = true;
        currentNodePosition.stationedUnit = null;
        transform.GetChild(1).gameObject.SetActive(false); //TODO: For the love of god find a better way to disable the textBox
        Destroy(this, 0.2f); //TODO: Either set this as inactive or create a dependency on a boolean isDead
    }

    public bool CanMove(int movementCost)
    {
        return currentMovementPoints >= movementCost;
    }

    public bool CanReachNode(Node targetNode)
    {
        List<Node> path = Pathfinding.Instance.FindPath(transform.position, targetNode.GridPosition, GridTracker.Instance.CombatGrid);
        
        return path != null && currentMovementPoints >= path.Count ;
    }

    public void UseMovement(int movementCost)
    {
        currentMovementPoints -= movementCost;
    }

    public void ReplenishMovementPoints()
    {
        currentMovementPoints = unitStats.movementSpeed;
        hasRetaliated = false;  // Reset retaliation status at the start of the turn.
    }

    public int CalculateDamage()
    {
        int damage = 0;
        if (stackSize <= 10)
        {
            for (int i = 0; i < stackSize; i++)
            {
                int individualDamage = Random.Range(unitStats.damageRange.x, unitStats.damageRange.y);
                damage += individualDamage;
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                int individualDamage = Random.Range(unitStats.damageRange.x, unitStats.damageRange.y);
                damage += individualDamage;
            }

            damage = (int)(damage * stackSize / 10);
        }
        return damage;  // This can later be adjusted based on attack/defense and other factors.
    }

    public List<Node> ReachableNodes()
    {
        List<Node> reachableNodes = new List<Node>();
        foreach (Node node in GridTracker.Instance.CombatGrid.Values)
        {
            if (CanReachNode(node))
            {
                reachableNodes.Add(node);
            }
        }
        return reachableNodes;
    }

    
}

