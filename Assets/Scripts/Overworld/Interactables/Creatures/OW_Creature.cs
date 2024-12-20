using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class OW_Creature : Interactable
{
    public Army neutralArmy = new Army();
    
    public override void InitializeInteractable(InitializeWorld e = null)
    {
        base.InitializeInteractable(e);
        neutralArmy.owner = new HeroInfo(0, neutralArmy);
        for (int i = 0; i < neutralArmy._units.Length; i++)
        {
            if(neutralArmy._units[i].stats == null)return;
            if(neutralArmy._units[i].identifier.Length <= 0)neutralArmy._units[i].identifier = neutralArmy._units[i].stats.unitName;
        }
    }

    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        OverworldEventBus<InitiateCombat>.Publish(new InitiateCombat(interactor.Army(), this.neutralArmy, interactor.gameObject, this.gameObject));
    }
    
}
