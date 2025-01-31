using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance;
    public GameObject pathfindingVisualizer;
    
    [Range(0, 1)]
    [SerializeField] private float distanceWeight = 0.5f;
    [Range(0, 1)]
    [SerializeField] private float arbitraryCostWeight = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    List<GameObject> pathfindingVisualizers = new List<GameObject>();
    public List<Node> FindPath(Vector2 start, Vector2 target, Dictionary<Vector2, Node> grid)
    {
        GeneralEventBus<StartPathGenEvent>.Publish(new StartPathGenEvent());
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
                GeneralEventBus<GeneratePathEvent>.Publish(new GeneratePathEvent(RetracePath(startNode, targetNode)));
                return RetracePath(startNode, targetNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            foreach (KeyValuePair<Node, float> neighbor in currentNode.neighbours)
            {
                if (!neighbor.Key.IsWalkable || closedList.Contains(neighbor.Key)) continue;
                float distanceToNeighbor = Vector2.Distance(currentNode.GridPosition, neighbor.Key.GridPosition);
                float arbitraryCost = neighbor.Value;
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
        Debug.Log("Pathfinder found no path");
        return null; 
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
        foreach (Node node in path)
        {
            if (pathfindingVisualizer == null) break;
            GameObject pV = Instantiate(pathfindingVisualizer, node.GridPosition, Quaternion.identity);
            pathfindingVisualizers.Add(pV);
        }
        return path;
    }

    [Button("Clear visuals", EButtonEnableMode.Playmode)]
    public void ClearVisualizers()
    {
        foreach (GameObject v in pathfindingVisualizers)
        {
            Destroy(v);
        }
    }

}
