using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwelling : FlaggableBuilding
{
    public Unit ProducedUnit;
    public int StationedAmont;
    
    public void AddUnit()
    {
        ProducedUnit.stackSize = 5; //TODO: Change this temp value to scale off of a preset system
        StationedAmont += ProducedUnit.stackSize;
    }

    public Unit RecruitUnit()
    {
        ProducedUnit.stackSize = StationedAmont;
        return ProducedUnit;
    }
}
