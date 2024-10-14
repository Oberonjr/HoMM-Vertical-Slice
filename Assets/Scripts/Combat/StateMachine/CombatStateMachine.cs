using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateMachine
{
    public ICombatState currentState { get; private set; }

    public void ChangeState(ICombatState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
    
    public void Update()
    {
        currentState?.UpdateState();
    }
}
