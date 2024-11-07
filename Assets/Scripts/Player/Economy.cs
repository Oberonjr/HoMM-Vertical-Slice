using System.Collections;
using System.Collections.Generic;


public class Economy
{
    public Dictionary<ResourceData.ResourceType, int> ResourceAmount { get; private set; }
    
    public Dictionary<ResourceData.ResourceType, int> DailyIncome { get; private set; }

    public Economy()
    {
        //TODO: Make economy initialization scale off of external factor
        ResourceAmount = new Dictionary<ResourceData.ResourceType, int>();
        ResourceAmount[ResourceData.ResourceType.Gold] = 10000;
        ResourceAmount[ResourceData.ResourceType.Ore] = 30;
        ResourceAmount[ResourceData.ResourceType.Wood] = 30;
        ResourceAmount[ResourceData.ResourceType.Crystal] = 10;
        DailyIncome = new Dictionary<ResourceData.ResourceType, int>
        {
            //TODO: Make this scale off of owned buildings from initialization, not having an actual ghost income based on nothing
            { ResourceData.ResourceType.Gold, 500 },
            { ResourceData.ResourceType.Ore, 2 },
            { ResourceData.ResourceType.Wood, 2 },
            { ResourceData.ResourceType.Crystal, 1 }
        };
    }

    public void UpdateCurrentResources()
    {
        foreach (ResourceData.ResourceType resource in DailyIncome.Keys)
        {
            if (ResourceAmount.ContainsKey(resource))
            {
                ResourceAmount[resource] += DailyIncome[resource];
            }
        }
    }
}
