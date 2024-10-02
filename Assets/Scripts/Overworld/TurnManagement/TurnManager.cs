using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public List<Player> players;  // List of players, configurable from Inspector.
    public Button endTurnButton;  // End Turn Button in UI.
    public Slider movementSlider; // UI Slider to show movement points.
    private int currentPlayerIndex = 0;
    private Player currentPlayer;

    public event Action OnPlayerTurnStart;
    public event Action OnPlayerTurnEnd;

    private void Start()
    {
        // Automatically add one human player if none or only one player is present.
        // if (players == null || players.Count <= 1)
        // {
        //     players = new List<Player>();
        //     Player humanPlayer = new Player { isAI = false };
        //     players.Add(humanPlayer);
        // }

        // Add end turn button functionality
        endTurnButton.onClick.AddListener(EndTurn);

        // Subscribe to key input for ending the turn
        //OnPlayerTurnStart += UpdateTurnUI;
        //OnPlayerTurnEnd += EndTurn;

        StartPlayerTurn();
    }

    private void Update()
    {
        // Also end turn when 'E' is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            EndTurn();
        }
    }

    private void StartPlayerTurn()
    {
        currentPlayer = players[currentPlayerIndex];
        OnPlayerTurnStart?.Invoke();

        // If it's an AI player, simulate their turn.
        if (currentPlayer.isAI)
        {
            StartCoroutine(SimulateAITurn());
        }
        else
        {
            // Replenish movement points for the human player.
            currentPlayer.ReplenishMovementPoints();
            UpdateMovementSlider();
        }
    }

    private void EndTurn()
    {
        OnPlayerTurnEnd?.Invoke();
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        StartPlayerTurn();
    }

    private IEnumerator SimulateAITurn()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Enemy AI turn being simulated.");
        EndTurn(); // AI automatically ends its turn.
    }

    private void UpdateMovementSlider()
    {
        if (!currentPlayer.isAI)
        {
            movementSlider.maxValue = currentPlayer.movementPoints;
            movementSlider.value = currentPlayer.movementPoints;
        }
    }

    private void UpdateTurnUI()
    {
        if (!currentPlayer.isAI)
        {
            // Enable the end turn button for human players.
            endTurnButton.interactable = true;
        }
        else
        {
            endTurnButton.interactable = false;
        }
    }
}
