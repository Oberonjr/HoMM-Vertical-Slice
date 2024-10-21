using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackingState : ICombatState
{
    private CombatStateMachine _stateMachine;
    private Unit attackerUnit;
    private Unit targetUnit;
    private bool isQueued;

    public UnitAttackingState(CombatStateMachine stateMachine, Unit attackerUnit, Unit targetUnit, bool isQueued)
    {
        this._stateMachine = stateMachine;
        this.attackerUnit = attackerUnit;
        this.targetUnit = targetUnit;
        this.isQueued = isQueued;
    }

    public void EnterState()
    {
        Debug.Log("Entered Unit Attacking State");
        CombatEventBus<AttackStartEvent>.Publish(new AttackStartEvent(attackerUnit, targetUnit));
        if (attackerUnit == CombatTurnManager.Instance.currentUnit && !isQueued)
        {
            Debug.Log("Ending attacker's turn");
            _stateMachine.ChangeState(new UnitTurnEndState(_stateMachine, attackerUnit));
        }
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        CombatEventBus<AttackEndEvent>.Publish(new AttackEndEvent(attackerUnit, targetUnit));
    }
}
