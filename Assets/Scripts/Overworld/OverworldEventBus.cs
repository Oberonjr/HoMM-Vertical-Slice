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

//Player related events
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

//Hero related events
public class OnHeroMoveStart : Event
{
    public HeroManager hero;

    public OnHeroMoveStart(Player pPlayer)
    {
        player = pPlayer;
    }
}
