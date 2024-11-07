using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldTurnManager : MonoBehaviour
{
    public static OverworldTurnManager Instance;

    public List<Player> CurrentPlayers = new List<Player>();
    public List<HeroManager> activeHeroes;  // TODO: Go away from using this for implementation, start relying on Player management
    public Slider movementSlider; // TODO: Remove this from here to a dedicated UIManager
    public Player ActivePlayer;
    
    private int currentPlayerIndex = 0;
    private HeroManager _currentHero;

    public event Action OnPlayerTurnStart;
    public event Action OnPlayerTurnEnd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            Debug.Log("Destroying extra OverworldTurnManager script");
        }

        Player player1 = new Player("Player1");
        CurrentPlayers.Add(player1);
        ActivePlayer = player1;
        foreach (HeroManager hero in activeHeroes)
        {
            player1.Heroes.Add(hero);
        }
    }

    private void Start()
    {
       

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
        _currentHero = activeHeroes[currentPlayerIndex];
        OnPlayerTurnStart?.Invoke();

        // If it's an AI player, simulate their turn.
        if (_currentHero.isAI)
        {
            StartCoroutine(SimulateAITurn());
        }
        else
        {
            // Replenish movement points for the human player.
            _currentHero.ReplenishMovementPoints();
            UpdateMovementSlider();
        }
    }

    private void EndTurn()
    {
        OnPlayerTurnEnd?.Invoke();
        currentPlayerIndex = (currentPlayerIndex + 1) % activeHeroes.Count;
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
        if (!_currentHero.isAI)
        {
            movementSlider.maxValue = _currentHero.movementPoints;
            movementSlider.value = _currentHero.movementPoints;
        }
    }
    
}
