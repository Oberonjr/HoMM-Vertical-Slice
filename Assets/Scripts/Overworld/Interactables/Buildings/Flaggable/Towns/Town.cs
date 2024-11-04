using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : FlaggableBuilding
{
    public Dictionary<ResourceData.ResourceType, int> ResourceAmountGenerated;

    void Start()
    {
        //TODO: Set income based on buildings present in Town
        ResourceAmountGenerated = new Dictionary<ResourceData.ResourceType, int>()
        {
            {ResourceData.ResourceType.Gold, 500},
            { ResourceData.ResourceType.Crystal , 0},
            { ResourceData.ResourceType.Ore , 0},
            { ResourceData.ResourceType.Wood , 0}
        };
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
    }
}
