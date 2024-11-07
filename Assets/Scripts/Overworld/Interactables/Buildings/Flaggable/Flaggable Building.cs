using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaggableBuilding : Building
{
    public Player owner;

    public override void Interact(OnHeroInteract e)
    {
        owner = e.hero.owner;
        owner.Kingdom.UpdateDailyIncome();
    }
}
