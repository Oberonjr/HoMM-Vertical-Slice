using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GeneralEventBus<T> where T : Event
{
    public static event Action<T> OnEvent;

    public static void Publish(T pEvent)
    {
        OnEvent?.Invoke(pEvent);
    }
}

public class StartPathGenEvent : Event
{
    public StartPathGenEvent()
    {
        
    }
}
public class GeneratePathEvent : Event
{
    public List<Node> path;

    public GeneratePathEvent(List<Node> path)
    {
        this.path = path;
    }
}
