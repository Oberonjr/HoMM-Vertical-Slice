using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : FlaggableBuilding
{
    public ResourceData.ResourceType ResourceType;
    public int AmountGenerated;

    public void AddIncome(Dictionary<ResourceData.ResourceType, int> income)
    {
        if (income.ContainsKey(this.ResourceType))
        {
            income[this.ResourceType] += AmountGenerated;
        }
    }
    
    public void LoseIncome(Dictionary<ResourceData.ResourceType, int> income)
    {
        if (income.ContainsKey(this.ResourceType))
        {
            income[this.ResourceType] -= AmountGenerated;
        }
    }

}
