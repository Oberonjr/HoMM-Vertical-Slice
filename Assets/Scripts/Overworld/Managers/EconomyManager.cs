using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager 
{
    public EconomyManager()
    {
        OverworldEventBus<OnPlayerTurnStart>.OnEvent += OnTurnStart;
    }

    private void OnTurnStart(OnPlayerTurnStart e)
    {
        e.player.Kingdom.Economy.UpdateCurrentResources();
    }
        
    ~EconomyManager()
    {
        OverworldEventBus<OnPlayerTurnStart>.OnEvent -= OnTurnStart;
    }
}
