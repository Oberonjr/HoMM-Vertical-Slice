using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class TownScreenLogic : MonoBehaviour
{
    [SerializeField] private FactionType factionType;
    
    [SerializedDictionary("TownBuildingData", "Associated Game Object")]
    public SerializedDictionary<TownBuildingData, GameObject> BuildingsMapping;

    public BuildPanelLogic buildPanel;
    
    [HideInInspector] public TownData currentTown;
    
    void OnEnable()
    {
        currentTown = OverworldUIManager.Instance.currentTown;
        foreach (TownBuildingData builtBuilding in currentTown.builtBuildings)
        {
            if (builtBuilding == null) return;
            if (!BuildingsMapping.ContainsKey(builtBuilding))
            {
                Debug.LogError($"Town screen of faction: {factionType} does not have building mapped for {builtBuilding.name}");
            }
            else
            {
                BuildingsMapping[builtBuilding].SetActive(true);
            }
        }
    }

    public void CloseTownScreen()
    {
        HeroMovementManager.Instance.allowInput = true;
        Destroy(gameObject);
    }
}
