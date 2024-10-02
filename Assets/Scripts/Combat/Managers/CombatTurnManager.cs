using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatTurnManager : MonoBehaviour
{
    public List<Unit> unitsInCombat = new List<Unit>();
    public int currentTurnIndex = 0;

    public static CombatTurnManager Instance;  // Singleton pattern.

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCombat();
    }

    public void StartCombat()
    {
        // Sort units by initiative.
        unitsInCombat = unitsInCombat.OrderByDescending(u => u.unitStats.initiative).ToList();
        StartUnitTurn();
    }

    public void StartUnitTurn()
    {
        if (currentTurnIndex >= unitsInCombat.Count)
        {
            currentTurnIndex = 0;
        }

        Unit currentUnit = unitsInCombat[currentTurnIndex];
        currentUnit.ReplenishMovementPoints();

        // If the unit is an AI, skip or handle its turn.
        if (currentUnit.IsAI)
        {
            SkipTurn();
        }
        else
        {
            // Enable player input for the current unit's turn.
        }
    }

    public void EndTurn()
    {
        currentTurnIndex++;
        StartUnitTurn();
    }

    public void SkipTurn()
    {
        // Handle skipping turn or unit's inaction.
        EndTurn();
    }
}

