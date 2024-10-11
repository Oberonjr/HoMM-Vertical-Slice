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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetMovement(Animator actor, bool isMoving)
    {
        actor.SetBool("Walking", isMoving);
    }

    void TriggerAttack(Animator actor)
    {
        actor.SetTrigger("Attack");
    }

    void TriggerDeath(Animator actor)
    {
        actor.SetTrigger("Dead");
    }

    void TriggerDamaged(Animator actor)
    {
        actor.SetTrigger("Damaged");
    }

    void TriggerFlex(Animator actor)
    {
        actor.SetTrigger("Flex");
    }

    void TriggerDefend(Animator actor)
    {
        actor.SetTrigger("Defend");
    }
    
    
}
