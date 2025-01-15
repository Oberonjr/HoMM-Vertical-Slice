using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreatureDwellingInfo 
{
    public UnitStats ProducedUnit;
    public int StationedAmont = 0;

    [HideInInspector] public bool isActive = false;
    
    public CreatureDwellingInfo()
    {
        //StationedAmont = ProducedUnit.Growth;
        OverworldEventBus<NewWeek>.OnEvent += AddUnitGrowth;
        //OverworldEventBus<RecruitUnit>.OnEvent += RemoveRecruitedUnit;
    }

    ~CreatureDwellingInfo()
    {
        OverworldEventBus<NewWeek>.OnEvent -= AddUnitGrowth;
        //OverworldEventBus<RecruitUnit>.OnEvent -= RemoveRecruitedUnit;
    }
    
    public void AddUnitGrowth(NewWeek e)
    {
        if(isActive) StationedAmont += ProducedUnit.Growth;
    }

    public void RemoveRecruitedUnit(int amount)
    {
        StationedAmont -= amount;
    }
    
    
}
