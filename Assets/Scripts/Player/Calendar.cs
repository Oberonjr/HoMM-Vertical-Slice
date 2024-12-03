using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar
{
    private static Calendar instance = null;
    private static readonly object padlock = new object();
    
    private Calendar(){}
    
    public int Day = 1;
    public int Week = 1;
    public int Month = 1;

    public static Calendar Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Calendar();
                }
                return instance;
            }
        }
    }

    public void AdvanceTime()
    {
        Day++;
        OverworldEventBus<NewDay>.Publish(new NewDay());
        if (Day >= 8)
        {
            Day = 1;
            Week++;
            OverworldEventBus<NewWeek>.Publish(new NewWeek());
            if (Week >= 5)
            {
                Week = 1;
                Month++;
                OverworldEventBus<NewMonth>.Publish(new NewMonth());
            }
        }
    }
}
