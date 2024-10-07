using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Dictionary<Vector2, Node> grid = new Dictionary<Vector2, Node>();
    public Vector3 tileSize; 
    public Color gridColor = Color.black; 
    public float lineWidth = 0.05f; 
    public GameObject temp;
    
    [SerializeReference]
    public INodePlotting nodePlotting;
    private Grid gridType;
    
    
    void Awake()
    {
        tileSize = tilemap.layoutGrid.cellSize;
        gridType = tilemap.layoutGrid;
        GenerateGrid();
        if(gridType.cellLayout == GridLayout.CellLayout.Rectangle) DrawGridLines();
    }

    
    void GenerateGrid()
    {
        List<GameObject> tempObjects = new List<GameObject>();
        int index = 0;
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);
            
            if (tile != null)
            {
                Vector2 gridPosition = nodePlotting.GetNodePosition(position, tileSize);;
                
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
        //Debug.Log(tile.name);
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

    
    
    Vector2 CalculateHexWorldPosition(float x, float y)
    {
        //TODO: Remove hard-set numbers, figure out the exact variables that slot in here
        float posX = y * tileSize.x + (x % 2 == 0 ? 0 : 1 / 2f); //0.8... is the current x-size of the hexagons in the grid

        // Calculate y position with the correct offset for staggered rows
        float posY = x * (tileSize.x * 0.75f); // Adjust row stagger
        
        return new Vector2(posX, posY);
    }

}
