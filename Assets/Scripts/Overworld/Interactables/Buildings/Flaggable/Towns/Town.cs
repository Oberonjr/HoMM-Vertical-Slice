using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

public class Town : FlaggableBuilding
{
    public TownData townData;
    
    public override void InitializeInteractable(InitializeWorld e = null)
    {
        base.InitializeInteractable(e);
        buildingType = BuildingType.TOWN;
        townData.InitializeTownData();
    }

    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        townData.ownerPlayer = owner;
        OverworldUIManager.Instance.currentTown = townData;
        OverworldEventBus<OpenTownScreen>.Publish(new OpenTownScreen(townData));
        HeroMovementManager.Instance.allowInput = false;
    }

    public void OnDestroy()
    {
        townData.DeinitializeTownData();
    }
}
