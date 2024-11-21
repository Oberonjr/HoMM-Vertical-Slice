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
    private List<Node> remainingPath;
    private Node selectedDestination;
    private bool pathShown;
    private Node currentNodePosition;

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
        turnManager.OnPlayerTurnStart += StartPlayerTurn;
        turnManager.OnPlayerTurnEnd += EndPlayerTurn;
    }

    void SnapToGridCenter()
    {
        Vector3Int playerGridPosition = GridManager.Instance.tilemap.WorldToCell(playerTransform.position);
        Vector3 snappedPosition = GridManager.Instance.tilemap.GetCellCenterWorld(playerGridPosition);
        playerTransform.position = snappedPosition;
        currentNodePosition = MyUtils.ClosestNode(snappedPosition);
    }

    //TODO: Move away from here
    void StartPlayerTurn()
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
    void EndPlayerTurn()
    {
        // Prevent movement at the end of the player's turn
        isMoving = false;
    }
    
    void Update()
    {
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
                    if (selectedDestination.placedInteractable != null)
                    {
                        //OverworldEventBus<OnHeroInteract>.Publish(new OnHeroInteract(hero, selectedDestination.placedInteractable));
                        selectedDestination.placedInteractable.Interact();
                        Debug.Log(selectedDestination.placedInteractable.name);
                    }
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
            hero.cHeroInfo.currentPosition = currentNodePosition;
            Destroy(node.spriteHighlight);
            hero.ConsumeMovementPoints(1);
            tilesMoved++;
            OverworldEventBus<OnHeroMoving>.Publish(new OnHeroMoving(hero, node));
            turnManager.movementSlider.value = hero.movementPoints;
        }

        if (path.Count > tilesMoved)
        {
            Debug.Log("Remaining path initialized");
            remainingPath = path.GetRange(tilesMoved, path.Count - tilesMoved);
            currentPath.Clear();
            foreach (Node node in remainingPath)
            {
                //Debug.Log(node.GridPosition);
            }
        }
        else
        {
            currentPath = new List<Node>(); // Clear path if the destination is reached.
            pathShown = false;
        }
        OverworldEventBus<OnHeroMoveEnd>.Publish(new OnHeroMoveEnd(hero, currentNodePosition));
        isMoving = false;
    }

}
