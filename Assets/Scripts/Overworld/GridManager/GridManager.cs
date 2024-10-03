using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Dictionary<Vector2, Node> grid;
    public Vector3 tileSize; // Automatically detected tile size
    public Color gridColor = Color.black; // Set grid line color
    public float lineWidth = 0.05f; // Thickness of the lines
    public GameObject temp;
    
    private Grid gridType;
    
    private Vector2[] directions = {
        Quaternion.AngleAxis(0, Vector3.forward) * Vector2.up, Quaternion.AngleAxis(60, Vector3.forward) * Vector2.up,
        Quaternion.AngleAxis(120, Vector3.forward) * Vector2.up, Quaternion.AngleAxis(180, Vector3.forward) * Vector2.up,
        Quaternion.AngleAxis(240, Vector3.forward) * Vector2.up, Quaternion.AngleAxis(300, Vector3.forward) * Vector2.up  
    };
    
    void Start()
    {
        tileSize = tilemap.layoutGrid.cellSize;
        gridType = tilemap.layoutGrid;
        GenerateGrid();
        if(gridType.cellLayout == GridLayout.CellLayout.Rectangle) DrawGridLines();
    }

    
    
    void GenerateGrid()
    {
        grid = new Dictionary<Vector2, Node>();
        List<GameObject> tempObjects = new List<GameObject>();
        int index = 0;
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);
            
            if (tile != null)
            {
                Vector2 gridPosition;
                if(gridType.cellLayout == GridLayout.CellLayout.Rectangle)  
                    gridPosition = new Vector2(position.x + tileSize.x /2, position.y + tileSize.y /2);
                else if (gridType.cellLayout == GridLayout.CellLayout.Hexagon)
                    gridPosition = CalculateHexWorldPosition(position.y, position.x);
                else
                {
                    Debug.Log("No valid grid found.");
                    gridPosition = new Vector2();
                }
                
                bool isWalkable = tile != null;
                int movementCost = GetMovementCost(tile);
                
                Node currentNode = new Node(gridPosition, isWalkable, movementCost);
                grid[gridPosition] = currentNode;
                //Instantiate(temp, new Vector3(gridPosition.x, gridPosition.y, 0), Quaternion.identity);


                GameObject tempObject = new GameObject();
                tempObject.name = "Location" + index;
                index++;
                tempObject.transform.position = gridPosition;
                tempObjects.Add(tempObject);
                BoxCollider2D nodeCollider = tempObject.AddComponent<BoxCollider2D>();
                nodeCollider.size = tileSize * 1.2f;
                //nodeCollider.isTrigger = true;
                
                currentNode.Collider = nodeCollider;
            }
        }

        var cols = new List<Collider2D>();
        
        foreach (Node currentNode in grid.Values)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.NoFilter();
            currentNode.Collider.OverlapCollider(filter, cols);

            cols.ForEach(col => currentNode.neighbours.Add(grid.Values.First(x => x.Collider.transform == col.transform),grid.Values.First(x => x.Collider.transform == col.transform).MovementCost));
            foreach (Node neighbour in currentNode.neighbours.Keys)
            {
                //Instantiate(temp, new Vector3(neighbour.GridPosition.x, neighbour.GridPosition.y, 0), Quaternion.identity);
            }
        }
        int tempObjectCount = tempObjects.Count;
        for (int i = 0; i < tempObjectCount; i++)
        {
            Destroy(tempObjects[i]);
        }
    }
    

    int GetMovementCost(TileBase tile)
    {
        Debug.Log(tile.name);
        if (tile == null) return int.MaxValue;
        switch (tile.name)
        {
            case "ForestTile": return 20;
            case "RoadTile": return 5;
            default: return 10;
        }
    }
    
    void DrawGridLines()
    {
        BoundsInt bounds = tilemap.cellBounds;

        // Draw vertical lines
        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                Vector3 startVertical = tilemap.CellToWorld(new Vector3Int(x, y, 0));
                Vector3 endVertical = tilemap.CellToWorld(new Vector3Int(x, y + 1, 0));
                DrawLine(startVertical, endVertical);
            }
        }

        // Draw horizontal lines
        for (int y = bounds.yMin; y <= bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x <= bounds.xMax; x++)
            {
                Vector3 startHorizontal = tilemap.CellToWorld(new Vector3Int(x, y, 0));
                Vector3 endHorizontal = tilemap.CellToWorld(new Vector3Int(x + 1, y, 0));
                DrawLine(startHorizontal, endHorizontal);
            }
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = tilemap.transform;
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = gridColor;
        lineRenderer.endColor = gridColor;
        lineRenderer.sortingOrder = 10; // Ensure it's rendered on top
    }

    public void GetNeighbors( Node node, Dictionary<Vector2, Node> grid)
    {
        if (gridType.cellLayout == GridLayout.CellLayout.Rectangle)
        {
            
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    AddNeighbors(node, grid, x, y);
                }
            } 
        }
        else if(gridType.cellLayout == GridLayout.CellLayout.Hexagon)
        {
            


            foreach (var dir in directions)
            {
                AddNeighbors(node, grid, node.GridPosition.x + dir.x, node.GridPosition.y + dir.y);
            }
        }
        else
        {
            Debug.Log("Provided grid is not rectangular nor hexagonal");
        }
    }

    void AddNeighbors(Node node, Dictionary<Vector2, Node> grid, float x, float y)
    {
        Vector2 neighborPosition;
        if(gridType.cellLayout == GridLayout.CellLayout.Rectangle)
             neighborPosition = new Vector2(node.GridPosition.x + (int)(x * tileSize.x), node.GridPosition.y + (int)(y * tileSize.y));
        else if (gridType.cellLayout == GridLayout.CellLayout.Hexagon)
            neighborPosition = CalculateHexWorldPosition(x, y);
        else
        {
            Debug.Log("Provided grid is not rectangular nor hexagonal");
            neighborPosition = new Vector2();
        }
        // Check if the neighbor position exists in the grid dictionary
        if (grid.ContainsKey(neighborPosition))
        {
            Node neighborNode = grid[neighborPosition];
            if (neighborNode == node || node.neighbours.ContainsKey(neighborNode)) return;
            var length = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
                    
            node.neighbours.Add(neighborNode, neighborNode.MovementCost * length);
            //Instantiate(temp, new Vector3(neighborPosition.x, neighborPosition.y, 0), Quaternion.identity);
        }
    }
    
    Vector2 CalculateHexWorldPosition(float x, float y)
    {
        //TODO: Remove hard-set numbers, figure out the exact variables that slot in here
        float posX = x * (0.8659766f * 0.8659766f); //0.8... is the current x-size of the hexagons in the grid

        // Calculate y position with the correct offset for staggered rows
        float posY = y * 0.8659766f + (x % 2 == 0 ? 0 : 1 / 2.3f); // Adjust row stagger
        
        return new Vector2(posX, posY);
    }

}
