using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class HeroManager : MonoBehaviour
{
    public bool isAI; 
    public int movementPoints;
    public int maxMovementPoints;

    public HeroInfo cHeroInfo;

    [HideInInspector] public Player owner;
    
    [SerializeField] private HeroStartingStats startingStats;
    

    private void Awake()
    {
        
        cHeroInfo = new HeroInfo(startingStats.MovementPoints, new Unit[7], startingStats.Name, startingStats.Icon, startingStats.AttackStat, startingStats.DefenseStat, startingStats.PowerStat, startingStats.KnowledgeStat);
        for (int i = 0; i < startingStats.StartingArmy.Count; i++)
        {
            cHeroInfo.Army[i] = startingStats.StartingArmy.ElementAt(i).Key;
            cHeroInfo.Army[i].stackSize = startingStats.StartingArmy.ElementAt(i).Value;
            cHeroInfo.Army[i].OwnerHero = this;
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

    public void AddUnit(Unit unit, int amount)
    {
        for (int i = 0; i < cHeroInfo.Army.Length; i++)
        {
            if (cHeroInfo.Army[i].UnitName == unit.UnitName)
            {
                cHeroInfo.Army[i].stackSize += amount;
                return;
            }
            else if (cHeroInfo.Army[i] == null)
            {
                cHeroInfo.Army[i] = unit;
                cHeroInfo.Army[i].stackSize = amount;
                cHeroInfo.Army[i].OwnerHero = this;
                return;
            }
            else
            {
                Debug.LogError($"{unit.UnitName} can't be added to {this.gameObject.name}'s army, army is full");
            }
        }
        
    }

    public void AddUnit(KeyValuePair<Unit, int> kvp)
    {
        AddUnit(kvp.Key, kvp.Value);
    }

    public void RemoveUnit(Unit unit, int amount)
    {
        for (int i = 0; i < cHeroInfo.Army.Length; i++)
        {
            if (cHeroInfo.Army[i].UnitName == unit.UnitName)
            {
                cHeroInfo.Army[i].stackSize = Mathf.Max(0, cHeroInfo.Army[i].stackSize - amount);
                if (cHeroInfo.Army[i].stackSize == 0)
                {
                    cHeroInfo.Army[i] = null;
                }
                return;
            }
            else
            {
                Debug.LogError($"{unit.UnitName} does not exist in {this.gameObject.name}'s army, something went wrong with the unit input");
            }
        }
    }

    public void RemoveUnit(KeyValuePair<Unit, int> kvp)
    {
        RemoveUnit(kvp.Key, kvp.Value);
    }
}

