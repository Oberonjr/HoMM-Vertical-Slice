using System.Collections;
using System.Collections.Generic;

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
}
