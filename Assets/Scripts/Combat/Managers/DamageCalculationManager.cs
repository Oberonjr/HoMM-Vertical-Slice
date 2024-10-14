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

    
}

