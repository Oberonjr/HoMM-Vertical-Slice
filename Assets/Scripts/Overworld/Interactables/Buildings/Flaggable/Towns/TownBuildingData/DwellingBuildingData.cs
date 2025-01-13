using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DwellingBuildingData", menuName = "TownScreen/DwellingBuildingData")]
public class DwellingBuildingData : TownBuildingData
{
    public CreatureDwellingInfo dwellingInfo;

    public override void OnBuild(TownData town)
    {
        base.OnBuild(town);
        // town.faction.factionTownUIScreen.GetComponent<TownScreenLogic>().BuildingsMapping[this]
        //     .GetComponent<RecruitButtonLogic>().dwellingInfo = dwellingInfo;
        // Step 1: Get the faction
        Faction faction = town.faction;
        if (faction == null)
        {
            Debug.LogError("Faction is null for the given town.");
            return;
        }

// Step 2: Get the faction's UI screen
        GameObject factionUIScreen = faction.factionTownUIScreen;
        if (factionUIScreen == null)
        {
            Debug.LogError("Faction UI screen is null for the faction: " + faction.name);
            return;
        }

// Step 3: Get the TownScreenLogic component
        TownScreenLogic townScreenLogic = factionUIScreen.GetComponent<TownScreenLogic>();
        if (townScreenLogic == null)
        {
            Debug.LogError("TownScreenLogic component is missing on the faction UI screen.");
            return;
        }

// Step 4: Check if this (current script's instance) exists in BuildingsMapping
        if (!townScreenLogic.BuildingsMapping.ContainsKey(this))
        {
            Debug.LogError("The current building is not mapped in BuildingsMapping.");
            return;
        }

// Step 5: Get the corresponding GameObject for this building
        GameObject buildingUIElement = townScreenLogic.BuildingsMapping[this];
        if (buildingUIElement == null)
        {
            Debug.LogError("Mapped UI element for the current building is null.");
            return;
        }

// Step 6: Get the RecruitButtonLogic component
        RecruitButtonLogic recruitButtonLogic = buildingUIElement.GetComponent<RecruitButtonLogic>();
        if (recruitButtonLogic == null)
        {
            Debug.LogError("RecruitButtonLogic component is missing on the mapped UI element.");
            return;
        }

// Step 7: Set the dwelling info
        recruitButtonLogic.dwellingInfo = dwellingInfo;
        Debug.Log("Successfully set the dwellingInfo for the RecruitButtonLogic.");

        dwellingInfo.StationedAmont = dwellingInfo.ProducedUnit.Growth;
    }
}
