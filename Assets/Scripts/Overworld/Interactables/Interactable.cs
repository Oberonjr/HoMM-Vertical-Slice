using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Node nodePosition;
    
    private void Start()
    {
        OverworldEventBus<InitializeWorld>.OnEvent += InitializeInteractable;
    }

    public virtual void Interact(HeroManager interactor)
    {
        OverworldEventBus<OnHeroInteract>.Publish(new OnHeroInteract(interactor, this));
    }

    public virtual void InitializeInteractable(InitializeWorld e = null)
    {
        nodePosition = MyUtils.ClosestNode(transform.position);
        transform.position = nodePosition.GridPosition;
        nodePosition.placedInteractable = this;
    }
    
    private void OnDestroy()
    {
        OverworldEventBus<InitializeWorld>.OnEvent -= InitializeInteractable;
    }
}
