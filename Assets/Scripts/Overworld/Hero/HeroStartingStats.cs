using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroStartingStats", menuName = "Interactables/Heroes")]
public class HeroStartingStats : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Unit[] StartingArmy;

    public int AttackStat;
    public int DefenseStat;
    public int PowerStat;
    public int KnowledgeStat;

    public int MovementPoints;
}
