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
        
        // path = Pathfinding.Instance.FindPath(currentUnit.currentNodePosition.GridPosition, destinationNode.GridPosition,
        //     GridManager.Instance.grid);
        // Debug.Log("The found path length is: " + path.Count);

    }

    public void UpdateState()
    {
        //CombatUnitMovement.Instance.StartCoroutine(CombatUnitMovement.Instance.MoveAlongPath(path));
    }

    public void ExitState()
    {
        
    }
    
    
}
