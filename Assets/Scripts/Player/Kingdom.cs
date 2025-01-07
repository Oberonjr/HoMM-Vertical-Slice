using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Kingdom
{
    public Economy Economy { get; private set; }
    public List<Town> Towns { get; private set; }
    public List<Dwelling> Dwellings { get; private set; }
    public List<Mine> Mines { get; private set; }

    public Kingdom()
    {
        Economy = new Economy();
        Towns = new List<Town>();
        Dwellings = new List<Dwelling>();
        Mines = new List<Mine>();
    }

    public void AddMine(Mine mine)
    {
        Mines.Add(mine);
    }

    public void AddDwelling(Dwelling dwelling)
    {
        Dwellings.Add(dwelling);
    }

    public void AddTown(Town town)
    {
        Towns.Add(town);
    }
    
    public void RemoveMine(Mine mine)
    {
        Mines.Remove(mine);
    }

    public void RemoveDwelling(Dwelling dwelling)
    {
        Dwellings.Remove(dwelling);
    }

    public void RemoveTown(Town town)
    {
        Towns.Remove(town);
    }

    public void UpdateDailyIncome()
    {
        foreach (KeyValuePair<ResourceData.ResourceType, int> kvp in Economy.DailyIncome.ToList())
        {
            Economy.DailyIncome[kvp.Key] = 0;
        }
        Dictionary<ResourceData.ResourceType, int> dailyIncome = Economy.DailyIncome;
        foreach (Town town in Towns)
        {
            town.townData.AddIncome(dailyIncome);
        }

        foreach (Mine mine in Mines)
        {
            mine.AddIncome(dailyIncome);
        }
    }
}
