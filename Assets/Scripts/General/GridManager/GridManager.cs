using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using NaughtyAttributes;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Dictionary<Vector2, Node> grid = new Dictionary<Vector2, Node>();
    public Vector3 tileSize; 
    public Color gridColor = Color.black; 
    public float lineWidth = 0.05f; 
    public GameObject temp;
    public GridPlottingStrategy gridPlottingStrategy;
    
    private Grid gridType;
    
    void Start()
    {
        tileSize = tilemap.layoutGrid.cellSize;
        gridType = tilemap.layoutGrid;
        GenerateGrid();
        if (gridType.cellLayout == GridLayout.CellLayout.Rectangle) DrawGridLines();
        

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
                Vector2 gridPosition = gridPlottingStrategy.GetNodePosition(position, tileSize);;
                
                bool isWalkable = tile != null;
                int movementCost = GetMovementCost(tile);
                
                Node currentNode = new Node(gridPosition, isWalkable, movementCost);
                grid[gridPosition] = currentNode;
                GameObject tempObject = new GameObject();
                tempObject.name = "Location" + index;
                index++;
                tempObject.transform.position = gridPosition;
                tempObjects.Add(tempObject);
                BoxCollider2D nodeCollider = tempObject.AddComponent<BoxCollider2D>();
                nodeCollider.size = tileSize * 1.2f;
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
        GeneralEventBus<GenerateGridEvent>.Publish(new GenerateGridEvent(grid));
        
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


    private List<GameObject> gridVisuals = new List<GameObject>();
    [Button("Generate Grid Visuals", EButtonEnableMode.Playmode)]
    public void VisualiseNodeGrid()
    {
        if(grid == null || grid.Count == 0) return;
        foreach (Node gridNode in grid.Values)
        {
           GameObject nodeHighlight = Instantiate(temp, gridNode.GridPosition, Quaternion.identity);
           gridVisuals.Add(nodeHighlight);
        }
    }

    [Button("Clear Grid Visuals", EButtonEnableMode.Playmode)]
    public void ClearGridVisuals()
    {
        foreach(GameObject gridVisual in gridVisuals){
            Destroy(gridVisual);    
        }
    }
}
