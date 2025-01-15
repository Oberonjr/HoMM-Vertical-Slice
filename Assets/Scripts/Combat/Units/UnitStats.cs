using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "Combat/UnitStats")]
public class UnitStats : ScriptableObject
{
    
    #region GeneralInfo
    [Header("General Info")]
    public string unitName;
    public Sprite icon;
    public GameObject prefab;
    #endregion
        
    #region CombatStats
    [Header("Combat Stats")]
    public int maxHP;
    public int attack;
    public int defense;
    public Vector2Int damageRange;  
    public int initiative;
    public int movementSpeed;
    #endregion
    
    #region EconomyStats
    [Header("Economy Stats")]
    public int Tier;
    public int Growth;
    public int Cost;

    #endregion

}

public interface IAbilities
{
        
}