using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateMachine
{
    public ICombatState currentState { get; private set; }

    public void ChangeState(ICombatState newState)
    {
        //Debug.Log("Exiting state: " + currentState);
        currentState?.ExitState();
        currentState = newState;
        //Debug.Log("Entering state: " + currentState);
        currentState.EnterState();
    }
    
    public void Update()
    {
        currentState?.UpdateState();
    }
}
