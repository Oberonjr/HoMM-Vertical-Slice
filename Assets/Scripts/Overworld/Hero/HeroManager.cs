using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class HeroManager : MonoBehaviour
{
    public bool isAI; 
    public int movementPoints;
    public int maxMovementPoints;


    
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

