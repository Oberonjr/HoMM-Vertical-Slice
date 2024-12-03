using System;
using UnityEngine;

public static class OverworldEventBus<T> where T : Event
{
    public static event Action<T> OnEvent;

    public static void Publish(T pEvent)
    {
        OnEvent?.Invoke(pEvent);
    }
}

//////////////////////////////////////General events
public class InitializeWorld : Event
{
    public InitializeWorld(){}
}

public class NewDay: Event{}

public class NewWeek: Event{}

public class NewMonth : Event{}


//////////////////////////////////////Player related events
//Turn management
public class OnPlayerTurnStart : Event
{
    public Player player;

    public OnPlayerTurnStart(Player pPlayer)
    {
        player = pPlayer;
    }
}

public class OnPlayerTurnEnd : Event
{
    public Player player;

    public OnPlayerTurnEnd(Player pPlayer)
    {
        player = pPlayer;
    }
}

//Economy
public class UpdateKindgomIncome : Event
{
    public Player player;

    public UpdateKindgomIncome(Player pPlayer)
    {
        player = pPlayer;
    }
}


//////////////////////////////////////Hero related events
//Movement
public class OnHeroMoveStart : Event
{
    public HeroManager hero;
    public Node startingLocation;

    public OnHeroMoveStart(HeroManager pHero, Node pNode)
    {
        hero = pHero;
        startingLocation = pNode;
    }
}

public class OnHeroMoveEnd : Event
{
    public HeroManager hero;
    public Node endingLocation;

    public OnHeroMoveEnd(HeroManager pHero, Node pNode)
    {
        hero = pHero;
        endingLocation = pNode;
    }
}

public class OnHeroMoving : Event
{
    public HeroManager hero;
    public Node currentLocation;

    public OnHeroMoving(HeroManager pHero, Node pNode)
    {
        hero = pHero;
        currentLocation = pNode;
    }
}

//Interaction
public class OnHeroInteract : Event
{
    public HeroManager hero;
    public Interactable interactable;

    public OnHeroInteract(HeroManager pHero, Interactable pInteractable)
    {
        hero = pHero;
        interactable = pInteractable;
    }
}
