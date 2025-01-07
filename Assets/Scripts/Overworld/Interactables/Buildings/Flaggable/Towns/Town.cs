using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

public class Town : FlaggableBuilding
{
    public TownData townData;
    public GameObject TownUIScreen; 
    
    public override void InitializeInteractable(InitializeWorld e = null)
    {
        base.InitializeInteractable(e);
        buildingType = BuildingType.TOWN;
    }

    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        TownUIScreen.SetActive(true);
        BuildPanelLogic.Instance.currentTown = townData;
        HeroMovementManager.Instance.allowInput = false;
    }
    
}
