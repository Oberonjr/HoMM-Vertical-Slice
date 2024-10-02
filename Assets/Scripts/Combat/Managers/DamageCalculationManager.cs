using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculationManager : MonoBehaviour
{
    public static DamageCalculationManager Instance;  // Singleton pattern.

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int CalculateDamage(Unit attacker, Unit defender)
    {
        int baseDamage = attacker.CalculateDamage();
        int finalDamage = Mathf.Max(1, baseDamage + (attacker.unitStats.attack - defender.unitStats.defense));  // Basic formula.
        return finalDamage;
    }

    public void ApplyDamage(Unit attacker, Unit defender)
    {
        int damage = CalculateDamage(attacker, defender);
        defender.TakeDamage(damage);

        if (!defender.hasRetaliated && defender.currentHP > 0)
        {
            attacker.TakeDamage(CalculateDamage(defender, attacker));
        }
    }
}

