using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager 
{
    public EconomyManager()
    {
        OverworldEventBus<OnPlayerTurnStart>.OnEvent += OnTurnStart;
        OverworldEventBus<UpdateKindgomIncome>.OnEvent += OnIncomeChanged;
    }

    private void OnTurnStart(OnPlayerTurnStart e)
    {
        e.player.Kingdom.Economy.UpdateCurrentResources();
    }

    private void OnIncomeChanged(UpdateKindgomIncome e)
    {
        e.player.Kingdom.UpdateDailyIncome();
    }
    
    ~EconomyManager()
    {
        OverworldEventBus<OnPlayerTurnStart>.OnEvent -= OnTurnStart;
        OverworldEventBus<UpdateKindgomIncome>.OnEvent -= OnIncomeChanged;
    }
}
