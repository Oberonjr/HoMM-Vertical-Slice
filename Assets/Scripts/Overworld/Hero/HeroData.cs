using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeroData
{
    public string Name;
    public Sprite Icon;
    public Unit[] Army = new Unit[7];

    public int AttackStat;
    public int DefenseStat;
    public int PowerStat;
    public int KnowledgeStat;

    public Node currentPosition;
}
