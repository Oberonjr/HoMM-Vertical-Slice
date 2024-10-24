using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Unit : MonoBehaviour
{
    public UnitStats unitStats;
    public int currentHP;
    public int currentMovementPoints;
    public bool hasRetaliated;  // Tracks if the unit has retaliated this round.
    public Node currentNodePosition;
    public bool isMoving;
    //public bool isAlive = true;
    public bool IsAI;
    public Action QueuedAction;
    
    [HideInInspector]public bool isUnitTurn;
    [HideInInspector]public Animator animator;
    
    private void Start()
    {
        currentHP = unitStats.maxHP;
        currentMovementPoints = unitStats.movementSpeed;
        CombatUnitMovement.Instance.SnapToGridCenter(this);
        currentNodePosition.stationedUnit = this;
        animator = GetComponentInChildren<Animator>() ?? throw new System.Exception($"No animator component found on {name}'s VFX child");
        
        
    }

    public void TakeDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        CombatEventBus<DamageReceivedEvent>.Publish(new DamageReceivedEvent(this));
        if (currentHP == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Handle unit death (remove from grid, remove from turn order, etc.)
        CombatTurnManager.Instance.unitsInCombat.Remove(this);
        CombatEventBus<UnitKilledEvent>.Publish(new UnitKilledEvent(this));
        Debug.Log(name + " has been killed");
        //isAlive = false;
        currentNodePosition.IsWalkable = true;
        currentNodePosition.stationedUnit = null;
        Destroy(this, 0.2f);
    }

    public bool CanMove(int movementCost)
    {
        return currentMovementPoints >= movementCost;
    }

    public bool CanReachNode(Node targetNode)
    {
        return  currentMovementPoints >= Pathfinding.Instance.FindPath(transform.position, targetNode.GridPosition, GridManager.Instance.grid).Count ;
    }

    public void UseMovement(int movementCost)
    {
        currentMovementPoints -= movementCost;
    }

    public void ReplenishMovementPoints()
    {
        currentMovementPoints = unitStats.movementSpeed;
        hasRetaliated = false;  // Reset retaliation status at the start of the turn.
    }

    public int CalculateDamage()
    {
        int damage = Random.Range(unitStats.damageRange.x, unitStats.damageRange.y);
        return damage;  // This can later be adjusted based on attack/defense and other factors.
    }

    public List<Node> ReachableNodes()
    {
        List<Node> reachableNodes = new List<Node>();
        foreach (Node node in GridManager.Instance.grid.Values)
        {
            if (CanReachNode(node))
            {
                reachableNodes.Add(node);
            }
        }
        return reachableNodes;
    }

    private void OnDisable()
    {
        
    }
}

