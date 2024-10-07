using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitMovement : MonoBehaviour
{
    private Unit currentUnit;
    private Dictionary<Vector2, Node> currentGrid;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentGrid = GridManager.grid;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    Node ClosestNode(Vector2 clickedPosition)
    {
        float shortestDistance = Mathf.Infinity;
        Node closestNode = null;
        foreach (Node node in currentGrid.Values)
        {
            float specificDistance = Vector2.Distance(node.GridPosition, clickedPosition);
            if (specificDistance < shortestDistance)
            {
                shortestDistance = specificDistance;
                closestNode = node;
            }
        }
        //Debug.Log("Mouse click position is: " + clickedPosition);
        //Debug.Log("Closest node attributed to that is: " + closestNode.GridPosition);
        return closestNode;
    }
}
