using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Interactable
{
    public ResourceData resource;
    public int ResourceAmount;

    public override void Interact(OnHeroInteract e)
    {
        base.Interact(e);
        OverworldTurnManager.Instance.ActivePlayer.Kingdom.Economy.ResourceAmount[resource.Resource] += ResourceAmount;
        Destroy(gameObject, 0.7f);
    }
}
