using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaggableBuilding : Building
{
    public Player owner;

    public enum BuildingType
    {
        TOWN,
        DWELLING,
        MINE
    };
    
    [HideInInspector] public BuildingType buildingType;

    public override void InitializeInteractable(InitializeWorld e = null)
    {
        base.InitializeInteractable(e);
    }
    
    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        owner = interactor.owner;
        AddBuilding();
        owner.Kingdom.UpdateDailyIncome();
    }

    void AddBuilding()
    {
        switch (buildingType)
        {
            case BuildingType.TOWN:
                owner.Kingdom.AddTown(this as Town);
                break;
            case BuildingType.DWELLING:
                owner.Kingdom.AddDwelling(this as Dwelling);
                break;
            case BuildingType.MINE:
                owner.Kingdom.AddMine (this as Mine);
                break;
            default:
                Debug.LogError ("Unknown building type");
                break;
        }
    }
}
