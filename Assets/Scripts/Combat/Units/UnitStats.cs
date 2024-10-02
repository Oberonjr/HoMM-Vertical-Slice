using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "Combat/UnitStats")]
public class UnitStats : ScriptableObject
{
    public string unitName;
    public int maxHP;
    public int attack;
    public int defense;
    public Vector2Int damageRange;  // Min and Max damage
    public int initiative;
    public int movementSpeed;
   
    public interface IAbilities
    {
        
    }
}
