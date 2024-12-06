using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class OW_Creature : Interactable
{

    [SerializedDictionary("Unit", "Amount")]
    public SerializedDictionary<Unit, int> _neutralArmyComp;
    [HideInInspector]public Army Army;
    
    public override void InitializeInteractable(InitializeWorld e = null)
    {
        base.InitializeInteractable(e);
        if (!(_neutralArmyComp.Count > 7))
        {
            for (int i = 0; i < _neutralArmyComp.Count; i++)
            {
                Army.AddUnit(_neutralArmyComp.ElementAt(i));
            }
        }
        else
        {
            throw new Exception("Too many units passed in the dictionary of neutral unit: " + gameObject.name);
        }
    }

    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        OverworldEventBus<InitiateCombat>.Publish(new InitiateCombat(interactor.Army(), this.Army));
    }
    
}
