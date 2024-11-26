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
        if (owner == null)
        {
            Debug.Log(interactor.owner.PlayerName + " is capturing neutral building " + gameObject.name);
            owner = interactor.owner;
            AddBuilding();
            OverworldEventBus<UpdateKindgomIncome>.Publish(new UpdateKindgomIncome(owner));
        }
        else if (owner != interactor.owner)
        {
            Debug.Log(interactor.owner.PlayerName + " is capturing " + gameObject.name + " from player " + owner.PlayerName);
           RemoveBuilding();
           OverworldEventBus<UpdateKindgomIncome>.Publish(new UpdateKindgomIncome(owner));
           owner = interactor.owner;
           AddBuilding();
           OverworldEventBus<UpdateKindgomIncome>.Publish(new UpdateKindgomIncome(owner));
        }
        
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
    
    void RemoveBuilding()
    {
        switch (buildingType)
        {
            case BuildingType.TOWN:
                owner.Kingdom.RemoveTown(this as Town);
                break;
            case BuildingType.DWELLING:
                owner.Kingdom.RemoveDwelling(this as Dwelling);
                break;
            case BuildingType.MINE:
                owner.Kingdom.RemoveMine (this as Mine);
                break;
            default:
                Debug.LogError ("Unknown building type");
                break;
        }
    }
}
