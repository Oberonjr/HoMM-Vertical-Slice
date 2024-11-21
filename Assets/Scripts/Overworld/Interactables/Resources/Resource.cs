using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Interactable
{
    public ResourceData resource;
    public int ResourceAmount;

    public override void Interact(HeroManager interactor)
    {
        base.Interact(interactor);
        interactor.owner.Kingdom.Economy.ResourceAmount[resource.Resource] += ResourceAmount;
        Destroy(gameObject);
    }
}
