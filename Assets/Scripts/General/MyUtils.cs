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
        if(GridTracker.Instance == null) throw new System.Exception("Grid Tracker is null");
        foreach (Node node in GridTracker.Instance.CurrentGrid.Values)
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
        if (Vector2.Distance(clickedPosition, closestNode.GridPosition) <= GridTracker.Instance.currentTilemap.layoutGrid.cellSize.x / 2)
        {
            return closestNode;
        }
        else
        {
            //Debug.Log("Clicked position is too far from closest eligible node.");
            return null;
        }
    }
    
    public static void SnapToGridCenter(Transform targetTransform, out Node objectNode)
    {
        Vector3Int playerGridPosition = GridTracker.Instance.currentTilemap.WorldToCell(targetTransform.position);
        Vector3 snappedPosition = GridTracker.Instance.currentTilemap.GetCellCenterWorld(playerGridPosition);
        targetTransform.position = snappedPosition;
        objectNode = ClosestNode(snappedPosition);
    }
}
