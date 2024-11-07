using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class HeroManager : MonoBehaviour
{
    public bool isAI; 
    public int movementPoints;
    public int maxMovementPoints;

    public HeroInfo cHeroInfo;

    [SerializeField] private HeroStartingStats startingStats;

    private void Awake()
    {
        cHeroInfo = new HeroInfo(startingStats.MovementPoints, new Unit[7], startingStats.Name, startingStats.Icon, startingStats.AttackStat, startingStats.DefenseStat, startingStats.PowerStat, startingStats.KnowledgeStat);
        for (int i = 0; i < startingStats.StartingArmy.Length; i++)
        {
            cHeroInfo.Army[i] = startingStats.StartingArmy[i];
        }
    }

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

