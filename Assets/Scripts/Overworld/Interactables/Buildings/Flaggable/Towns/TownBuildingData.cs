using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "BuildingData", menuName = "TownScreen/BuildingData")]
public class TownBuildingData : ScriptableObject
{
    public string name;
    public Sprite sprite;
    
    [SerializedDictionary("Resource type", "Value")]
    public SerializedDictionary<ResourceData.ResourceType, int> cost;

    public TownBuildingData[] prerequisites;
    public string description;
    
    
}
