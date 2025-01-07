using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[Serializable]
public class TownData
{
    public Faction faction;

    public Player ownerPlayer;
    
    [SerializedDictionary("Resouce", "Amount")]
    public SerializedDictionary<ResourceData.ResourceType, int> ResourceAmountGenerated;
    
    public List<TownBuildingData> builtBuildings;
    
    void ChangeIncome(ResourceData.ResourceType resourceType, int newAmount)
    {
        ResourceAmountGenerated[resourceType] = newAmount;
        Debug.Log("Changing the " + resourceType + " produced by town " + faction.factionType + " to: " + newAmount);
    }
    
    public void AddIncome(Dictionary<ResourceData.ResourceType, int> income)
    {
        Debug.Log("Adding the income of town: " + faction.factionType);
        foreach (KeyValuePair<ResourceData.ResourceType, int> resourceIncome in ResourceAmountGenerated)
        {
            income[resourceIncome.Key] += resourceIncome.Value;
        }
    }
    
    public void LoseIncome(Dictionary<ResourceData.ResourceType, int> income)
    {
        Debug.Log("Losing the income of town: " + faction.factionType);
        foreach (KeyValuePair<ResourceData.ResourceType, int> resourceIncome in ResourceAmountGenerated)
        {
            income[resourceIncome.Key] -= resourceIncome.Value;
        }
    }
}
