using System.Collections;
using System.Collections.Generic;


public class Player
{
    public string PlayerName { get; private set; }
    public Kingdom Kingdom { get; private set; }
    public List<HeroManager> Heroes { get; private set; }

    public Player(string playerName)
    {
        PlayerName = playerName;
        Kingdom = new Kingdom();
        Heroes = new List<HeroManager>();
        
        
    }

    public void AddHeroManager(HeroManager hero)
    {
        Heroes.Add(hero);
    }
    
    public void RemoveHeroManager(HeroManager hero)
    {
        Heroes.Remove(hero);
    }
}
