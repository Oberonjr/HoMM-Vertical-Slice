using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[Serializable]
public class HeroInfo
{
    public string Name;
    public Sprite Icon;
    public Unit[] Army;

    public int AttackStat;
    public int DefenseStat;
    public int PowerStat;
    public int KnowledgeStat;

    public int MovementPoints;

    public Node currentPosition;

    public HeroInfo(int pMovementPoints, Unit[] pArmy, string pName = "NoName Mcgee", Sprite pIcon = null,  int pAttack = 0, int pDefense = 0, int pPower = 0, int pKnowledge = 0)
    {
        Name = pName;
        Icon = pIcon;
        Army = pArmy;
        MovementPoints = pMovementPoints;
        AttackStat = pAttack;
        DefenseStat = pDefense;
        PowerStat = pPower;
        KnowledgeStat = pKnowledge;
    }
}
