using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaggableBuilding : Building
{
    public Player owner;

    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        owner = interactor.owner;
        owner.Kingdom.UpdateDailyIncome();
    }
}
