using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Node nodePosition;
    
    private void Start()
    {
        nodePosition = MyUtils.ClosestNode(transform.position);
        transform.position = nodePosition.GridPosition;
        nodePosition.placedInteractable = this;
    }

    public virtual void Interact(HeroManager interactor)
    {
        OverworldEventBus<OnHeroInteract>.Publish(new OnHeroInteract(interactor, this));
    }

    private void OnDestroy()
    {
        
    }
}
