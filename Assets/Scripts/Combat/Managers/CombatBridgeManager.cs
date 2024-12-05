using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBridgeManager : MonoBehaviour
{
    public static CombatBridgeManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    
}
