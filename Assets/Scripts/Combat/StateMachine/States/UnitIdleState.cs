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
        
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        
    }
}
