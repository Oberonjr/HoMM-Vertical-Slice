using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyUtils
{
    public static IEnumerator LateStart(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
    
    public static Node ClosestNode(Vector2 clickedPosition)
    {
        float shortestDistance = Mathf.Infinity;
        Node closestNode = null;
        if(GridManager.Instance == null) throw new System.Exception("Grid Manager is null");
        foreach (Node node in GridManager.Instance.grid.Values)
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
        if (Vector2.Distance(clickedPosition, closestNode.GridPosition) <= GridManager.Instance.tileSize.x / 2)
        {
            return closestNode;
        }
        else
        {
            //Debug.Log("Clicked position is too far from closest eligible node.");
            return null;
        }
        
    }
}
