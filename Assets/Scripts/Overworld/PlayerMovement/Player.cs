using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isAI; // Is this player controlled by AI?
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

