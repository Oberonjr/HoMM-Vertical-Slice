using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class UnitSlot 
{
    public UnitStats stats;
    public int amount;
    
    public GameObject unitPrefab;

    //[HideInInspector]
    public string identifier;

    public GUID realIdentifier;
    
    public UnitSlot(UnitStats pStats, int pAmount, GameObject pUnitPrefab = null)
    {
        stats = pStats;
        amount = pAmount;
        unitPrefab = pUnitPrefab;
        if(stats != null)identifier = stats.unitName;
    }
}

