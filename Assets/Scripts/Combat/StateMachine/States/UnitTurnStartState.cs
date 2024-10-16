using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTurnStartState : ICombatState
{
    private CombatStateMachine stateMachine;
    private Unit currentUnit;

    public UnitTurnStartState(CombatStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        currentUnit = CombatTurnManager.Instance.currentUnit;
        //Debug.Log("Unit " + currentUnit.name + " is starting turn at current node position: " + currentUnit.currentNodePosition.GridPosition.ToString());
    }

    public void EnterState()
    {
        CombatEventBus<UnitTurnStartEvent>.Publish(new UnitTurnStartEvent(currentUnit));
        stateMachine.ChangeState(new UnitIdleState(stateMachine, currentUnit));
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        
    }
    
    
}
