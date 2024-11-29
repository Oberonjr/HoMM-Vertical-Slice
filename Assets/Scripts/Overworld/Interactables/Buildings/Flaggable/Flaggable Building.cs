using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaggableBuilding : Building
{
    public Player owner;
    public Dictionary<ResourceData.ResourceType, int> dailyIncome = new Dictionary<ResourceData.ResourceType, int>();
    public Dictionary<Unit, int> weeklyProduction = new Dictionary<Unit, int>();
    
    private Kingdom _kingdom;
    private Economy _economy;
    
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
        if (owner != null)
        {
            _kingdom = owner.Kingdom;
            _economy = owner.Kingdom.Economy;
        }
    }
    
    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        ChangeOwnership(interactor.owner);
        
    }

    void ChangeOwnership(Player newOwner)
    {
        if (owner == null)
        {
            Debug.Log(newOwner.PlayerName + " is capturing neutral building " + gameObject.name);
            owner = newOwner;
            AddBuilding();
            OverworldEventBus<UpdateKindgomIncome>.Publish(new UpdateKindgomIncome(owner));
        }
        else if (owner != newOwner)
        {
            Debug.Log(newOwner.PlayerName + " is capturing " + gameObject.name + " from player " + owner.PlayerName);
            RemoveBuilding();
            OverworldEventBus<UpdateKindgomIncome>.Publish(new UpdateKindgomIncome(owner));
            owner = newOwner;
            AddBuilding();
            OverworldEventBus<UpdateKindgomIncome>.Publish(new UpdateKindgomIncome(owner));
        }
        _kingdom = newOwner.Kingdom;
        _economy = newOwner.Kingdom.Economy;
    }


    void AddDailyIncome()
    {
        
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
