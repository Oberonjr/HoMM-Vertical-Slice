using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hex_Star_Pathfinding : MonoBehaviour
{
    
        // public Tilemap hexTilemap; // Reference to the hexagonal tilemap.
        // public Dictionary<Vector2Int, Node> hexGrid;
        //
        // public void GenerateHexGrid()
        // {
        //     hexGrid = new Dictionary<Vector2Int, Node>();
        //
        //     // Iterate over all tiles in the hexTilemap and create a grid of Nodes
        //     foreach (var position in hexTilemap.cellBounds.allPositionsWithin)
        //     {
        //         TileBase tile = hexTilemap.GetTile(position);
        //         if (tile != null)
        //         {
        //             Vector2Int gridPosition = new Vector2Int(position.x, position.y);
        //             bool isWalkable = true; // Set walkable tiles.
        //             hexGrid[gridPosition] = new Node(gridPosition, isWalkable, movementCost: 10); // Example cost.
        //         }
        //     }
        // }
        //
        // public List<Node> GetHexNeighbors(Node node)
        // {
        //     List<Node> neighbors = new List<Node>();
        //     Vector2Int[] directions = {
        //         new Vector2Int(1, 0), new Vector2Int(-1, 0),
        //         new Vector2Int(0, 1), new Vector2Int(0, -1),
        //         new Vector2Int(1, -1), new Vector2Int(-1, 1)  // Diagonal neighbors
        //     };
        //
        //     foreach (var dir in directions)
        //     {
        //         Vector2Int neighborPos = node.GridPosition + dir;
        //         if (hexGrid.ContainsKey(neighborPos))
        //         {
        //             neighbors.Add(hexGrid[neighborPos]);
        //         }
        //     }
        //
        //     return neighbors;
        // }
    }

