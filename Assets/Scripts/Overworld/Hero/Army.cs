using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class Army 
{
    [SerializedDictionary("Unit", "Count")]
    public SerializedDictionary<Unit, int> armyUnits;
    
     
}
