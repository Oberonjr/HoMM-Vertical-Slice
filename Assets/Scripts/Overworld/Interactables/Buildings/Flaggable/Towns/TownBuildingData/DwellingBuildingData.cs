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
        town.faction.factionTownUIScreen.GetComponent<TownScreenLogic>().BuildingsMapping[this]
            .GetComponent<RecruitButtonLogic>().dwellingInfo = dwellingInfo;
        dwellingInfo.StationedAmont = dwellingInfo.ProducedUnit.Growth;
    }
}
