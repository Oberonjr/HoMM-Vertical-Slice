using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwelling : FlaggableBuilding
{
    public Unit ProducedUnit;
    public int StationedAmont = 0;
    
    public override void InitializeInteractable(InitializeWorld e = null)
    {
        base.InitializeInteractable(e);
        buildingType = BuildingType.DWELLING;
        StationedAmont = ProducedUnit.unitStats.Growth;
        OverworldEventBus<NewWeek>.OnEvent += AddUnitGrowth;
    }

    void OnDestroy()
    {
        OverworldEventBus<NewWeek>.OnEvent -= AddUnitGrowth;
    }

    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        OverworldEventBus<OpenRecruitScreen>.Publish(new OpenRecruitScreen(ProducedUnit, StationedAmont));
    }
    
    public void AddUnitGrowth(NewWeek e)
    {
        StationedAmont += ProducedUnit.unitStats.Growth;
    }

    public KeyValuePair<Unit, int> RecruitUnit()
    {
        return new KeyValuePair<Unit, int>(ProducedUnit, StationedAmont);
    }
}
