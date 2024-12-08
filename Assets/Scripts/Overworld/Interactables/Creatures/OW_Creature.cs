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
    }

    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        OverworldEventBus<InitiateCombat>.Publish(new InitiateCombat(interactor.Army(), this.neutralArmy));
    }
    
}
