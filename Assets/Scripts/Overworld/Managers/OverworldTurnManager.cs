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
    private EconomyManager _economyManager;
    private Calendar _calendar;

    //TODO: Remove these and switch to EventBus's events
    

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

        //TODO: Reformat this testing mess into a system that actually makes sense
        Player player1 = new Player("Player1");
        CurrentPlayers.Add(player1);
        ActivePlayer = player1;
        foreach (HeroManager hero in activeHeroes)
        {
            player1.Heroes.Add(hero);
            //Debug.Log("Added hero to player1");
        }

        foreach (HeroManager ownedHero in player1.Heroes)
        {
            ownedHero.owner = player1;
        }
    }

    private void Start()
    {
        StartCoroutine(MyUtils.LateStart(0.2f, () =>
        {
            OverworldEventBus<InitializeWorld>.Publish(new InitializeWorld());
            if(_economyManager == null)_economyManager = new EconomyManager();
            if(_calendar == null) _calendar = Calendar.Instance;
            StartPlayerTurn(); 
        }));
        
    }

    private void Update()
    {
        // Also end turn when 'E' is pressed
        if (Input.GetKeyDown(KeyCode.E) && !HeroMovementManager.Instance.isMoving && HeroMovementManager.Instance.allowInput)
        {
            EndTurn();
        }
    }

    private void StartPlayerTurn()
    {
        foreach (Player player in CurrentPlayers)
        {
            if(!player.hasPlayedTurn) continue;
            _calendar.AdvanceTime();
        }
        _currentHero = activeHeroes[currentPlayerIndex];
        OverworldEventBus<OnPlayerTurnStart>.Publish(new OnPlayerTurnStart(ActivePlayer));
        _currentHero.ReplenishMovementPoints();
        UpdateMovementSlider();
        
    }

    private void EndTurn()
    {
        ActivePlayer.hasPlayedTurn = true;
        
        OverworldEventBus<OnPlayerTurnEnd>.Publish(new OnPlayerTurnEnd(ActivePlayer));
        currentPlayerIndex = (currentPlayerIndex + 1) % activeHeroes.Count;
        StartPlayerTurn();
    }
    

    private void UpdateMovementSlider()
    {
        movementSlider.maxValue = _currentHero.movementPoints;
        movementSlider.value = _currentHero.movementPoints;
    }
    
}
