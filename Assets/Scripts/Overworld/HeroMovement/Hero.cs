using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Hero : MonoBehaviour
{
    public bool isAI; 
    public int movementPoints;
    public int maxMovementPoints;

    public Unit[] Army = new Unit[7];
    
    public void ReplenishMovementPoints()
    {
        movementPoints = maxMovementPoints;
    }

    public void ConsumeMovementPoints(int points)
    {
        movementPoints = Mathf.Max(0, movementPoints - points);
    }

    public bool CanMove(int requiredPoints)
    {
        return movementPoints >= requiredPoints;
    }
}

