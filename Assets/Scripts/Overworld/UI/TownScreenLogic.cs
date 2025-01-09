using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class TownScreenLogic : MonoBehaviour
{
    [SerializeField] private FactionType factionType;
    
    [SerializedDictionary("TownBuildingData", "Associated Game Object")]
    public SerializedDictionary<TownBuildingData, GameObject> BuildingsMapping;

    [HideInInspector] public TownData currentTown;
}
