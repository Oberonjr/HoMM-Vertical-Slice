using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeroMovementManager : MonoBehaviour
{
    public static HeroMovementManager Instance;
    
    public Pathfinding pathfinding;
    public Transform playerTransform;
    public float animationSpeed = 3f;
    public GameObject pathSpritePrefab; // Sprite for visualizing the path
    public GameObject destinationSpritePrefab; // Sprite for the destination
    public OverworldTurnManager turnManager; //TODO: remove refference as it's Singleton
    public HeroManager hero;
    
    [HideInInspector]public bool isMoving;
    
    private List<Node> currentPath;
    private List<Node> remainingPath; //TODO: Tie to UIManager
    private Node selectedDestination;
    private bool pathShown;           //TODO: Tie to UIManager
    private bool canArrive;
    private Node currentNodePosition;
    private Queue<Action> actionQueue = new Queue<Action>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Hero Movement Instance already exists! Destroying game object.");
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        SnapToGridCenter();
        OverworldEventBus<OnPlayerTurnStart>.OnEvent += StartPlayerTurn; //TODO: MOVE THIS!!!
        OverworldEventBus<OnPlayerTurnEnd>.OnEvent += EndPlayerTurn; //TODO: MOVE THIS TOO!!!
        OverworldEventBus<OnHeroMoveEnd>.OnEvent += ProcessActionQueue;

    }

    void OnDestroy()
    {
        OverworldEventBus<OnPlayerTurnStart>.OnEvent -= StartPlayerTurn; //TODO: need I say more...
        OverworldEventBus<OnPlayerTurnEnd>.OnEvent -= EndPlayerTurn;
        OverworldEventBus<OnHeroMoveEnd>.OnEvent -= ProcessActionQueue;
    }
    
    void SnapToGridCenter()
    {
        Vector3Int playerGridPosition = GridManager.Instance.tilemap.WorldToCell(playerTransform.position);
        Vector3 snappedPosition = GridManager.Instance.tilemap.GetCellCenterWorld(playerGridPosition);
        playerTransform.position = snappedPosition;
        currentNodePosition = MyUtils.ClosestNode(snappedPosition);
    }

    //TODO: Move away from here
    void StartPlayerTurn(OnPlayerTurnStart e)
    {
        // If it's this player's turn, allow movement.
        if (!hero.isAI)
        {
            hero.ReplenishMovementPoints(); // Replenish movement points at start of the turn
            ClearPreviousPath();
            if (remainingPath != null && remainingPath.Any())
            {
                Debug.Log("Re-applying remainder path");
                currentPath.AddRange(remainingPath);
                VisualizePath(currentPath);
                remainingPath.Clear();
                foreach (Node n in currentPath)
                {
                    Debug.Log(n.GridPosition);
                }
            }
        }
    }

    //TODO: Move away from here
    void EndPlayerTurn(OnPlayerTurnEnd e)
    {
        // Prevent movement at the end of the player's turn
        isMoving = false;
    }
    
    void Update()
    {
        //TODO: Move this to OverworldInputManager
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            // Allow destination setting if player has no movement points left.
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Node clickedNode = MyUtils.ClosestNode(mousePosition);
            //Debug.Log("Click");
            if (clickedNode != null && clickedNode.IsWalkable)
            {
                //Debug.Log("Tile is valid");
                if (clickedNode == selectedDestination && pathShown)
                {
                    StartCoroutine(MoveAlongPath(currentPath));
                    
                }
                else
                {
                    // Generate path 
                    selectedDestination = clickedNode;
                    currentPath = pathfinding.FindPath(currentNodePosition.GridPosition, clickedNode.GridPosition, GridManager.Instance.grid);

                    if (currentPath != null)
                    {
                        ClearPreviousPath();
                        VisualizePath(currentPath);
                    
                    }
                    pathShown = true;
                    if (selectedDestination.placedInteractable != null)
                    {
                        currentPath.Remove(currentPath.Last());
                    }
                }
                
            }
        }
    }
    //TODO: Move to UI Manager
    void VisualizePath(List<Node> path)
    {
        //Debug.Log("Visualizing path");
        for (int i = 0; i < path.Count; i++)
        {
            // Skip placing a sprite where the player is already positioned
            if (path[i] == currentNodePosition)
            {
                continue;
            }

            Vector3 targetPosition = path[i].GridPosition;
            GameObject pathSprite;
            if (i == path.Count - 1)
            {
                 pathSprite = Instantiate(destinationSpritePrefab, targetPosition, Quaternion.identity);
            }
            else
            {
                 pathSprite = Instantiate(pathSpritePrefab, targetPosition, Quaternion.identity);
            }
            
            path[i].spriteHighlight = pathSprite;
            // Darken sprites if the path exceeds movement range
            if (i >= hero.movementPoints)
            {
                pathSprite.GetComponent<SpriteRenderer>().color = Color.gray;
            }
            
        }
    }
    //TODO: Move to UI Manager
    void ClearPreviousPath()
    {
        //Debug.Log("Clearing previous path");
        List<GameObject> currentPathSprites = new List<GameObject>();
        foreach (Node node in GridManager.Instance.grid.Values)
        {
            if (node.spriteHighlight != null)
            {
                currentPathSprites.Add(node.spriteHighlight);
            }
        }
        foreach (GameObject pathSprite in currentPathSprites)
        {
            Destroy(pathSprite);
        }
    }
    
    
    IEnumerator MoveAlongPath(List<Node> path)
    {
        isMoving = true;
        int tilesMoved = 0;
        OverworldEventBus<OnHeroMoveStart>.Publish(new OnHeroMoveStart(hero, currentNodePosition));
        foreach (Node node in path)
        {
            if (!hero.CanMove(1))
            {
                break;
            }

            Vector3 targetPosition = node.GridPosition;
            while (Vector3.Distance(playerTransform.position, targetPosition) > Mathf.Epsilon)
            {
                playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPosition, animationSpeed * Time.deltaTime);
                yield return null;
            }
            currentNodePosition = node;
            //TODO: Move everything below to a function on Hero/UIM
            hero.cHeroInfo.currentPosition = currentNodePosition;
            Destroy(node.spriteHighlight);
            hero.ConsumeMovementPoints(1);
            tilesMoved++;
            turnManager.movementSlider.value = hero.movementPoints;
            OverworldEventBus<OnHeroMoving>.Publish(new OnHeroMoving(hero, node));
        }

        //TODO: Make UIM listen to an event for this to handle this logic
        if (path.Count > tilesMoved)
        {
            Debug.Log("Remaining path initialized");
            remainingPath = path.GetRange(tilesMoved, path.Count - tilesMoved);
            currentPath.Clear();
            foreach (Node node in remainingPath)
            {
                //Debug.Log(node.GridPosition);
            }

            canArrive = false;
        }
        else
        {
            currentPath = new List<Node>(); // Clear path if the destination is reached.
            pathShown = false;
            canArrive = true;
        }
        if (selectedDestination.placedInteractable != null && canArrive)
        {
            EnqueueInteraction(selectedDestination.placedInteractable);
        }
        OverworldEventBus<OnHeroMoveEnd>.Publish(new OnHeroMoveEnd(hero, currentNodePosition));
        isMoving = false;
    }

    void EnqueueInteraction(Interactable interactable){
        //Debug.Log(currentNodePosition.neighbours.ContainsKey(selectedDestination)); 
        actionQueue.Enqueue(() => interactable.Interact(hero));
    }
    
    void ProcessActionQueue(OnHeroMoveEnd e)
    {
        if (actionQueue.Count > 0)
        {
            Action interaction = actionQueue.Dequeue();
            interaction?.Invoke();
        }
        
    }
}
