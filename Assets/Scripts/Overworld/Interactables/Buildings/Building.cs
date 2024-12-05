using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Interactable
{
    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
    }

    public override void InitializeInteractable(InitializeWorld e = null)
    {
        base.InitializeInteractable(e);
    }
}
