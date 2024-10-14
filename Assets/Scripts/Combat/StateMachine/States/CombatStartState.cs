using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatStartState : ICombatState
{
    private CombatStateMachine stateMachine;

    public CombatStartState(CombatStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void EnterState()
    {
        CombatEventBus<CombatStartEvent>.Publish(new CombatStartEvent());
        stateMachine.ChangeState(new UnitTurnStartState(stateMachine));
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        
    }
    
    
}
