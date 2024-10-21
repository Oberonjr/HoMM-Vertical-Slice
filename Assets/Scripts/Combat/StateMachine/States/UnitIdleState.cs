using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : ICombatState
{
    private CombatStateMachine stateMachine;
    private Unit currentUnit;

    public UnitIdleState(CombatStateMachine stateMachine, Unit currentUnit)
    {
        this.stateMachine = stateMachine;
        this.currentUnit = currentUnit;
    }

    public void EnterState()
    {
        CombatTurnManager.Instance.indicator.SetActive(true);
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        CombatTurnManager.Instance.ClearHighlightedSprites();
        CombatTurnManager.Instance.indicator.SetActive(false);
    }
}
