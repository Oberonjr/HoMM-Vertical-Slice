using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "IncomeBuildingData", menuName = "TownScreen/IncomeBuildingData")]
public class IncomeBuildingData : TownBuildingData
{
    [SerializedDictionary("ResourceType", "Amount")]
    public SerializedDictionary<ResourceData.ResourceType, int> income;
    
    public override void OnBuild(TownData town)
    {
        base.OnBuild(town);
        foreach (KeyValuePair<ResourceData.ResourceType, int> kvp in income)
        {
            ResourceData.ResourceType resourceType = kvp.Key;
            if (town.ResourceAmountGenerated.ContainsKey(resourceType))
            {
                town.ResourceAmountGenerated[resourceType] += kvp.Value;
            }
        }
        OverworldEventBus<UpdateKindgomIncome>.Publish(new UpdateKindgomIncome(town.ownerPlayer));
    }
}
