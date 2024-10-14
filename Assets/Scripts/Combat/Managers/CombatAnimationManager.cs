using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationManager : MonoBehaviour
{
    public static CombatAnimationManager Instance;

    
    private Animator actorAnimator;
    private Animator reactorAnimator;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        CombatEventBus<UnitStartMovingEvent>.OnEvent += SetMovementTrue;
        CombatEventBus<UnitEndMovingEvent>.OnEvent += SetMovementFalse;
        CombatEventBus<AttackStartEvent>.OnEvent += TriggerAttack;
        CombatEventBus<DamageReceivedEvent>.OnEvent += TriggerDamaged;
    }
    
    void SetMovementTrue(UnitStartMovingEvent e)
    {
        e.unit.animator.SetBool("Walking", true);
    }
    
    void SetMovementFalse(UnitEndMovingEvent e)
    {
        e.unit.animator.SetBool("Walking", false);
    }

    void TriggerAttack(AttackStartEvent e)
    {
        e.attacker.animator.SetTrigger("Attack");
    }

    void TriggerDeath(Animator actor)
    {
        actor.SetTrigger("Dead");
    }

    void TriggerDamaged(DamageReceivedEvent e)
    {
        e.target.animator.SetTrigger("Damaged");
    }

    void TriggerFlex(Animator actor)
    {
        actor.SetTrigger("Flex");
    }

    void TriggerDefend(Animator actor)
    {
        actor.SetTrigger("Defend");
    }

    private void OnDisable()
    {
        CombatEventBus<UnitStartMovingEvent>.OnEvent -= SetMovementTrue;
        CombatEventBus<UnitEndMovingEvent>.OnEvent -= SetMovementFalse;
        CombatEventBus<AttackStartEvent>.OnEvent -= TriggerAttack;
        CombatEventBus<DamageReceivedEvent>.OnEvent -= TriggerDamaged;
    }
}
