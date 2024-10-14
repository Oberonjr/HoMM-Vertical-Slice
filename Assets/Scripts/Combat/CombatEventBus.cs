using System;
using UnityEngine;

public abstract class Event{}

public static class CombatEventBus<T> where T : Event
{
    public static event Action<T> OnEvent;

    public static void Publish(T pEvent)
    {
        OnEvent?.Invoke(pEvent);
    }
}

    public class AttackStartEvent : Event
    {
        public Unit attacker;
        public Unit defender;
        public int damage;

        public AttackStartEvent(Unit pAttacker, Unit pDefender, int pDamage)
        {
            attacker = pAttacker;
            defender = pDefender;
            damage = pDamage;
        }
    }

    public class AttackEndEvent : Event
    {
        public Unit attacker;
        public Unit defender;

        public AttackEndEvent(Unit pAttacker, Unit pDefender)
        {
            attacker = pAttacker;
            defender = pDefender;
        }
    }
    
    // Damage received
    public class DamageReceivedEvent : Event
    {
        public Unit target;
        
        public DamageReceivedEvent(Unit pTarget)
        {
            target = pTarget;
        }
    }

    // Movement events
    public class UnitStartMovingEvent : Event
    {
        public Unit unit;
        public bool isMoving;

        public UnitStartMovingEvent(Unit pUnit)
        {
            unit = pUnit;
            isMoving = true;
        }
    }

    public class UnitEndMovingEvent : Event
    {
        public Unit unit;
        public bool isMoving;
        
        
        public UnitEndMovingEvent(Unit pUnit)
        {
            unit = pUnit;
            isMoving = false;
        }
    }

    public class UnitMovedEvent : Event
    {
        public Unit unit;
        public Node currentLocation;

        public UnitMovedEvent(Unit pUnit, Node pCurrentLocation)
        {
            unit = pUnit;
            currentLocation = pCurrentLocation;
        }
    }

    // Turn events
    public class UnitTurnStartEvent : Event
    {
        public Unit unit;

        public UnitTurnStartEvent(Unit pUnit)
        {
            unit = pUnit;
        }
    }

    public class UnitTurnEndEvent : Event
    {
        public Unit unit;

        public UnitTurnEndEvent(Unit pUnit)
        {
            unit = pUnit;
        }
    }

    // Retaliation
    public class UnitRetaliateEvent : Event
    {
        public Unit attacker;
        public Unit defender;

        public UnitRetaliateEvent(Unit pAttacker, Unit pDefender)
        {
            attacker = pAttacker;
            defender = pDefender;
        }
    }

    // Mouse events
    public class MouseHoverEvent : Event
    {
        public Node position;

        public MouseHoverEvent(Node pPosition)
        {
            position = pPosition;
        }
    }

    public class MouseLeftClickEvent : Event
    {
        public Node position;

        public MouseLeftClickEvent(Node pPosition)
        {
            position = pPosition;
        }
    }

    public class MouseRightClickEvent : Event
    {
        public Node position;

        public MouseRightClickEvent(Node pPosition)
        {
            position = pPosition;
        }
    }

    // Unit status events
    public class UnitKilledEvent : Event
    {
        public Unit unit;

        public UnitKilledEvent(Unit pUnit)
        {
            unit = pUnit;
        }
    }

    public class UnitStackReducedEvent : Event
    {
        public Unit unit;
        public int reducedBy;

        public UnitStackReducedEvent(Unit pUnit, int pReducedBy)
        {
            unit = pUnit;
            reducedBy = pReducedBy;
        }
    }

    // Combat start/end
    public class CombatStartEvent : Event { }

    public class CombatEndEvent : Event { }

