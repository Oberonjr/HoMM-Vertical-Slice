using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreatureDwellingInfo 
{
    public UnitStats ProducedUnit;
    public int StationedAmont = 0;

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
        StationedAmont += ProducedUnit.Growth;
    }

    public void RemoveRecruitedUnit(int amount)
    {
        StationedAmont -= amount;
    }
    
    
}
