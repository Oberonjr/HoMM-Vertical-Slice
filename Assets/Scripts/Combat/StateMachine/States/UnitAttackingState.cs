using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackingState : ICombatState
{
    private CombatStateMachine stateMachine;
    private Unit attackerUnit;
    private Unit targetUnit;

    public UnitAttackingState(CombatStateMachine stateMachine, Unit attackerUnit, Unit targetUnit)
    {
        this.stateMachine = stateMachine;
        this.attackerUnit = attackerUnit;
        this.targetUnit = targetUnit;
    }

    public void EnterState()
    {
        Debug.Log("Entered Unit Attacking State");
        CombatEventBus<AttackStartEvent>.Publish(new AttackStartEvent(attackerUnit, targetUnit));
        if (attackerUnit == CombatTurnManager.Instance.currentUnit)
        {
            Debug.Log("Ending attacker's turn");
            stateMachine.ChangeState(new UnitTurnEndState(stateMachine, attackerUnit));
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
