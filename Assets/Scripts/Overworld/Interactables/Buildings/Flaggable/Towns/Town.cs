using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

public class Town : FlaggableBuilding
{
    [SerializedDictionary("Resouce", "Amount")]
    public SerializedDictionary<ResourceData.ResourceType, int> ResourceAmountGenerated;
    public GameObject TownUIScreen; 
    
    public override void InitializeInteractable(InitializeWorld e = null)
    {
        base.InitializeInteractable(e);
        buildingType = BuildingType.TOWN;
        //TODO: Set income based on buildings present in Town
        ResourceAmountGenerated = new SerializedDictionary<ResourceData.ResourceType, int>()
        {
            {ResourceData.ResourceType.Gold, 500},
            { ResourceData.ResourceType.Crystal , 0},
            { ResourceData.ResourceType.Ore , 0},
            { ResourceData.ResourceType.Wood , 0}
        };
    }

    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        TownUIScreen.SetActive(true);
    }

    void ChangeIncome(ResourceData.ResourceType resourceType, int newAmount)
    {
        ResourceAmountGenerated[resourceType] = newAmount;
        Debug.Log("Changing the " + resourceType + " produced by town " + name + " to: " + newAmount);
    }
    
    public void AddIncome(Dictionary<ResourceData.ResourceType, int> income)
    {
        Debug.Log("Adding the income of town: " + name);
        foreach (KeyValuePair<ResourceData.ResourceType, int> resourceIncome in ResourceAmountGenerated)
        {
            income[resourceIncome.Key] += resourceIncome.Value;
        }
    }
    
    public void LoseIncome(Dictionary<ResourceData.ResourceType, int> income)
    {
        Debug.Log("Losing the income of town: " + name);
        foreach (KeyValuePair<ResourceData.ResourceType, int> resourceIncome in ResourceAmountGenerated)
        {
            income[resourceIncome.Key] -= resourceIncome.Value;
        }
    }

    //Temporary functions meant for testing of the town income
    public void BuildResourceSilo()
    {
        ChangeIncome(ResourceData.ResourceType.Ore, 1);
        ChangeIncome(ResourceData.ResourceType.Wood, 1);
    }

    public void BuildCrystalCavern()
    {
        ChangeIncome(ResourceData.ResourceType.Crystal, 1);
    }

    public void BuildTownHall()
    {
        ChangeIncome(ResourceData.ResourceType.Gold, 1000);
        OverworldEventBus<UpdateKindgomIncome>.Publish(new UpdateKindgomIncome(owner));
        owner.Kingdom.Economy.SpendResource(ResourceData.ResourceType.Gold, 1000);
    }
}
