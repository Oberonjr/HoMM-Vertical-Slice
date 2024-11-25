using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : FlaggableBuilding
{
    public ResourceData.ResourceType ResourceType;
    public int AmountGenerated;


    public override void InitializeInteractable(InitializeWorld e = null)
    {
        base.InitializeInteractable(e);
        buildingType = BuildingType.MINE;
    }
    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
    }
    
    public void AddIncome(Dictionary<ResourceData.ResourceType, int> income)
    {
        if (income.ContainsKey(this.ResourceType))
        {
            income[this.ResourceType] += AmountGenerated;
        }
        else
        {
            throw new Exception("Provided economy does not have the specified resource type in mine: " + name);
        }
    }
    
    public void LoseIncome(Dictionary<ResourceData.ResourceType, int> income)
    {
        if (income.ContainsKey(this.ResourceType))
        {
            income[this.ResourceType] -= AmountGenerated;
        }
        else
        {
            throw new Exception("Provided economy does not have the specified resource type in mine: " + name);
        }
    }

}
