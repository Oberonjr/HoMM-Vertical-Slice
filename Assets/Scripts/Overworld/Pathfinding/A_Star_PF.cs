using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class A_Star_PF : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float distanceWeight = 0.5f;
    [Range(0, 1)]
    [SerializeField] private float arbitraryCostWeight = 0.5f;

    public List<Node> FindPath(Vector2 start, Vector2 target, Dictionary<Vector2, Node> grid)
    {
        
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

                // Calculate distance between current node and neighbor node
                float distanceToNeighbor = Vector2.Distance(currentNode.GridPosition, neighbor.Key.GridPosition);

                // Calculate the arbitrary cost from the other script (neighbor.Value in this case)
                float arbitraryCost = neighbor.Value;

                // Calculate the final movement cost using both the distance and arbitrary cost, weighted
                float newMovementCostToNeighbor = 
                    currentNode.GCost 
                    + (distanceToNeighbor * distanceWeight) 
                    + (arbitraryCost * arbitraryCostWeight);
                if (newMovementCostToNeighbor < neighbor.Key.GCost || !openList.Contains(neighbor.Key))
                {
                    neighbor.Key.GCost = newMovementCostToNeighbor;
                    neighbor.Key.HCost = (distanceToNeighbor * distanceWeight) + (arbitraryCost * arbitraryCostWeight);
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
