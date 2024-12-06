using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatBridgeManager : MonoBehaviour
{
    public static CombatBridgeManager Instance;

    public Army AggressorArmy; // { get; private set; }
    public Army DefenderArmy; // { get; private set; }
    
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

    void Start()
    {
        OverworldEventBus<InitiateCombat>.OnEvent += InitiateCombat;
    }

    void OnDestroy()
    {
        OverworldEventBus<InitiateCombat>.OnEvent -= InitiateCombat;
    }

    public void InitiateCombat(InitiateCombat e)
    {
        AggressorArmy = e.AggressorArmy;
        DefenderArmy = e.DefenderArmy;
        
        SceneManager.LoadScene("CombatScene");
    }
}
