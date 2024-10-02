using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public A_Star_PF pathfinding;
    public GridManager gridManager;
    public Transform playerTransform;
    public float moveSpeed = 3f;
    public GameObject pathSpritePrefab; // Sprite for visualizing the path
    public GameObject destinationSpritePrefab; // Sprite for the destination
    public TurnManager turnManager;
    public Player player;
    private List<Node> currentPath;
    private List<Node> remainingPath;
    private bool isMoving = false;
    private Vector2Int selectedDestination;
    private bool pathShown = false;
    

    
    void Start()
    {
        SnapToGridCenter();
        turnManager.OnPlayerTurnStart += StartPlayerTurn;
        turnManager.OnPlayerTurnEnd += EndPlayerTurn;
    }

    void SnapToGridCenter()
    {
        Vector3Int playerGridPosition = gridManager.tilemap.WorldToCell(playerTransform.position);
        Vector3 snappedPosition = gridManager.tilemap.GetCellCenterWorld(playerGridPosition);
        playerTransform.position = snappedPosition;
    }

    void StartPlayerTurn()
    {
        // If it's this player's turn, allow movement.
        if (!player.isAI)
        {
            player.ReplenishMovementPoints(); // Replenish movement points at start of the turn
            ClearPreviousPath();
            if (remainingPath != null && remainingPath.Any())
            {
                Debug.Log("Re-applying remainder path");
                currentPath = remainingPath;
                VisualizePath(currentPath);
                remainingPath.Clear();
            }
        }
    }

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
            Vector3Int clickedCell = gridManager.tilemap.WorldToCell(mousePosition);
            Vector2Int clickedTile = new Vector2Int(clickedCell.x, clickedCell.y);
            Debug.Log("Click");
            if (gridManager.grid.ContainsKey(clickedTile) && gridManager.grid[clickedTile].IsWalkable)
            {
                Debug.Log("Tile is valid");
                if (clickedTile == selectedDestination && pathShown)
                {
                    StartCoroutine(MoveAlongPath(currentPath));
                    pathShown = false;
                }
                else
                {
                    // Generate path 
                    selectedDestination = clickedTile;
                    currentPath = pathfinding.FindPath(new Vector2Int(Mathf.RoundToInt(playerTransform.position.x), Mathf.RoundToInt(playerTransform.position.y)), clickedTile);

                    if (currentPath != null)
                    {
                        ClearPreviousPath();
                        VisualizePath(currentPath);
                    
                    }
                    pathShown = true;
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
            if (path[i].GridPosition == new Vector2Int(Mathf.RoundToInt(playerTransform.position.x), Mathf.RoundToInt(playerTransform.position.y)))
            {
                continue;
            }

            Vector3 targetPosition = gridManager.tilemap.GetCellCenterWorld(new Vector3Int((int)path[i].GridPosition.x, (int)path[i].GridPosition.y, 0));
            GameObject pathSprite = Instantiate(pathSpritePrefab, targetPosition, Quaternion.identity);
        
            // Darken sprites if the path exceeds movement range
            if (i >= player.movementPoints)
            {
                pathSprite.GetComponent<SpriteRenderer>().color = Color.gray;
            }

            if (i == path.Count - 1) // Last node is the destination
            {
                GameObject destinationSprite = Instantiate(destinationSpritePrefab, targetPosition, Quaternion.identity);
                if (i >= player.movementPoints)
                {
                    destinationSprite.GetComponent<SpriteRenderer>().color = Color.gray; // Darken if out of range
                }
            }
        }
    }


    void ClearPreviousPath()
    {
        //Debug.Log("Clearing previous path");
        foreach (GameObject pathSprite in GameObject.FindGameObjectsWithTag("PathSprite"))
        {
            Destroy(pathSprite);
        }
    }

    IEnumerator MoveAlongPath(List<Node> path)
    {
        isMoving = true;
        int tilesMoved = 0;

        foreach (var node in path)
        {
            if (!player.CanMove(1)) break;

            Vector3 targetPosition = gridManager.tilemap.GetCellCenterWorld(new Vector3Int((int)node.GridPosition.x, (int)node.GridPosition.y, 0));
            while (Vector3.Distance(playerTransform.position, targetPosition) > 0.01f)
            {
                playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            player.ConsumeMovementPoints(1);
            tilesMoved++;
            turnManager.movementSlider.value = player.movementPoints;
        }

        if (path.Count > tilesMoved)
        {
            Debug.Log("Remaining path initialized");
            remainingPath = path.GetRange(tilesMoved, path.Count - tilesMoved); // Save remaining path for future turns.
        }
        else
        {
            currentPath = new List<Node>(); // Clear path if the destination is reached.
        }

        isMoving = false;
    }

}
