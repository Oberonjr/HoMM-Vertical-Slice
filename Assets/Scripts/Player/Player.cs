using System.Collections;
using System.Collections.Generic;


public class Player
{
    public string PlayerName { get; private set; }
    public Kingdom Kingdom { get; private set; }
    public List<Hero> Heroes { get; private set; }

    public Player(string playerName)
    {
        PlayerName = playerName;
        Kingdom = new Kingdom();
        Heroes = new List<Hero>();
    }

    public void AddHero(Hero hero)
    {
        Heroes.Add(hero);
    }
    
    public void RemoveHero(Hero hero)
    {
        Heroes.Remove(hero);
    }
}
