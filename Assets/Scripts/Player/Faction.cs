using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public enum FactionType
{
    CASTLE,
    RAMPART,
    TOWER,
    INFERNO,
    NECROPOLIS,
    DUNGEON,
    STRONGHOLD,
    FORTRESS,
    CONFLUX,
    COVE,
    FACTORY
}
[CreateAssetMenu(fileName = "FactionData", menuName = "Factions/FactionData")]
public class Faction : ScriptableObject
{
    public FactionType factionType;

    public GameObject factionTownUIScreen;

    [SerializedDictionary("Building Data", "Visual Object")]
    public SerializedDictionary<TownBuildingData, GameObject> factionTownBuildings;

    public List<UnitStats> factionUnits;
}
