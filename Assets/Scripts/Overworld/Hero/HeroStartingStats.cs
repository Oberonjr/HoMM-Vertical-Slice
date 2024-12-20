using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "HeroStartingStats", menuName = "Interactables/Heroes")]
public class HeroStartingStats : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Army StartingArmy;

    public int AttackStat;
    public int DefenseStat;
    public int PowerStat;
    public int KnowledgeStat;

    public int MovementPoints;
}
