using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;


public class TownBuildingData : ScriptableObject
{
    public string name;
    public string description;
    public Sprite buildScreenSprite;
    
    [SerializedDictionary("Resource type", "Amount")]
    public SerializedDictionary<ResourceData.ResourceType, int> cost;

    public TownBuildingData[] prerequisites;
    public TownBuildingData buildingToReplace;
    public TownBuildingData buildingToEnable;

    private bool canBeBuilt = true;
    
    public virtual void OnBuild(TownData town)
    {
        town.builtBuildings.Add(this);
        if(town.builtBuildings.Contains(buildingToReplace)) town.builtBuildings.Remove(buildingToReplace);
        town.ownerPlayer.Kingdom.Economy.SpendResource(cost);
        canBeBuilt = false;
    }

    public bool CanBeBuilt(TownData town)
    {
        foreach (TownBuildingData building in prerequisites)
        {
            if(!town.builtBuildings.Contains(building))
            {
                return false;
            }
        }

        if (!town.ownerPlayer.Kingdom.Economy.CanSpendResource(cost))
        {
            return false;  
        }
        return canBeBuilt;
    }
}
