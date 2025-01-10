using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwelling : FlaggableBuilding
{
    public CreatureDwellingInfo dwellingInfo;
    
    
    public override void InitializeInteractable(InitializeWorld e = null)
    {
        base.InitializeInteractable(e);
        buildingType = BuildingType.DWELLING;
        dwellingInfo.StationedAmont = dwellingInfo.ProducedUnit.Growth;
    }

    

    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        OverworldEventBus<OpenRecruitScreen>.Publish(new OpenRecruitScreen(dwellingInfo));
    }
   
}
