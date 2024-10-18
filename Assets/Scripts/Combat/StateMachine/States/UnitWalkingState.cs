using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWalkingState : ICombatState
{
    private CombatStateMachine stateMachine;
    private Unit currentUnit;
    private Node destinationNode;
    
    private List<Node> path;

    public UnitWalkingState(CombatStateMachine stateMachine, Unit currentUnit, Node destinationNode)
    {
        this.stateMachine = stateMachine;
        this.currentUnit = currentUnit;
        this.destinationNode = destinationNode;
    }

    public void EnterState()
    {
        CombatEventBus<UnitStartMovingEvent>.Publish(new UnitStartMovingEvent(currentUnit, destinationNode.GridPosition));
    }

    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        //Debug.Log("Exiting Unit Walking State");
    }
    
    
}
