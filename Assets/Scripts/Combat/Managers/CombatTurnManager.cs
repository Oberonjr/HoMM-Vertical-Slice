using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatTurnManager : MonoBehaviour
{
    public static CombatTurnManager Instance;  
    
    
    public List<Unit> unitsInCombat = new List<Unit>();
    public int currentTurnIndex = 0;


    private Unit currentUnit;
    private void Awake()
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

    private void Start()
    {
        StartCoroutine(MyUtils.LateStart(0.1f, () =>
        {
            StartCombat();
        }));

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

        currentUnit = unitsInCombat[currentTurnIndex];
        Debug.Log("Starting the turn of: " + currentUnit.name);
        currentUnit.currentNodePosition.IsWalkable = true;
        currentUnit.isUnitTurn = true;
        if (currentUnit.IsAI)
        {
            SkipTurn();
        }
        else
        {
            
            CombatUnitMovement.Instance.currentUnit = currentUnit;
        }
        
        currentUnit.ReplenishMovementPoints();
    }

    public void EndTurn()
    {
        Debug.Log("Ending the turn of: " + currentUnit.name);
        currentUnit.currentNodePosition.IsWalkable = false;
        currentUnit.isUnitTurn = false;
        currentTurnIndex++;
        StartUnitTurn();
    }

    public void SkipTurn()
    {
        Debug.Log("Skipping the turn of: " + currentUnit.name);
        EndTurn();
    }
}

