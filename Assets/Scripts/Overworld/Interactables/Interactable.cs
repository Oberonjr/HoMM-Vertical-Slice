using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Node nodePosition;
    
    private void Start()
    {
        OverworldEventBus<OnHeroInteract>.OnEvent += Interact;
        nodePosition = MyUtils.ClosestNode(transform.position);
        transform.position = nodePosition.GridPosition;
        nodePosition.placedInteractable = this;
    }

    public virtual void Interact(OnHeroInteract e)
    {
        
    }

    private void OnDestroy()
    {
        OverworldEventBus<OnHeroInteract>.OnEvent -= Interact;
    }
}
