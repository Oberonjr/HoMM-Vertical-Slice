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
            { ResourceData.ResourceType.Gold, 0 },
            { ResourceData.ResourceType.Ore, 0 },
            { ResourceData.ResourceType.Wood, 0 },
            { ResourceData.ResourceType.Crystal, 0 }
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

    public void AddResource(ResourceData.ResourceType resource, int amount)
    {
        ResourceAmount[resource] += amount;
    }

    public void AddResource(Dictionary<ResourceData.ResourceType, int> resourceAmount)
    {
        foreach (KeyValuePair<ResourceData.ResourceType, int> kvp in resourceAmount)
        {
            if (ResourceAmount.ContainsKey(kvp.Key))
            {
                ResourceAmount[kvp.Key] += kvp.Value;
            }
        }
    }

    public void SpendResource(ResourceData.ResourceType resource, int amount)
    {
        if (CanSpendResource(resource, amount))
        {
            ResourceAmount[resource] -= amount;
        }
    }

    public void SpendResource(Dictionary<ResourceData.ResourceType, int> resourceAmount)
    {
        if (CanSpendResource(resourceAmount))
        {
            foreach (KeyValuePair<ResourceData.ResourceType, int> kvp in resourceAmount)
            {
                ResourceAmount[kvp.Key] -= kvp.Value;
            }
        }
    }

    public bool CanSpendResource(ResourceData.ResourceType resource, int amount)
    {
        return ResourceAmount.ContainsKey(resource) && ResourceAmount[resource] >= amount;
    }

    public bool CanSpendResource(Dictionary<ResourceData.ResourceType, int> resourceAmount)
    {
        List<bool> enoughResources = new List<bool>();
        foreach (KeyValuePair<ResourceData.ResourceType, int> kvp in resourceAmount)
        {
            if (ResourceAmount.ContainsKey(kvp.Key))
            {
                enoughResources.Add(ResourceAmount[kvp.Key] >= kvp.Value);
            }
        }

        return !enoughResources.Contains(false);
    }
}
