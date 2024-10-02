using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class A_Star_PF : MonoBehaviour
{
    public GridManager gridManager;

    public List<Node> FindPath(Vector2Int start, Vector2Int target)
    {
        Dictionary<Vector2, Node> grid = gridManager.grid;

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        Node startNode = grid[start];
        Node targetNode = grid[target];

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList.OrderBy(n => n.FCost).ThenBy(n => n.HCost).First();

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (KeyValuePair<Node, float> neighbor in currentNode.neighbours)
            {
                if (!neighbor.Key.IsWalkable || closedList.Contains(neighbor.Key)) continue;

                float newMovementCostToNeighbor = currentNode.GCost + neighbor.Value;
                if (newMovementCostToNeighbor < neighbor.Key.GCost || !openList.Contains(neighbor.Key))
                {
                    neighbor.Key.GCost = newMovementCostToNeighbor;
                    neighbor.Key.HCost = neighbor.Value;
                    neighbor.Key.ParentNode = currentNode;

                    if (!openList.Contains(neighbor.Key))
                        openList.Add(neighbor.Key);
                }
            }
        }

        return null; // No path found
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.ParentNode;
        }

        path.Reverse();
        return path;
    }

    

}
