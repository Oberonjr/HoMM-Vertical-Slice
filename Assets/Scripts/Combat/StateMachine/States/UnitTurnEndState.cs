using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTurnEndState : ICombatState
{
    private CombatStateMachine stateMachine;
    private Unit currentUnit;

    public UnitTurnEndState(CombatStateMachine stateMachine, Unit currentUnit)
    {
        this.stateMachine = stateMachine;
        this.currentUnit = currentUnit;
    }

    public void EnterState()
    {
        //Debug.Log("Ending turn of: " + currentUnit.name);
        CombatEventBus<UnitTurnEndEvent>.Publish(new UnitTurnEndEvent(currentUnit));
        stateMachine.ChangeState(new UnitTurnStartState(stateMachine));
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        
    }
}
