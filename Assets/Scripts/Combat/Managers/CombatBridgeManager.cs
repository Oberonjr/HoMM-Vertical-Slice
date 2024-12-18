using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatBridgeManager : MonoBehaviour
{
    [SerializeField] private GameObject Overworld;
    public static CombatBridgeManager Instance;

    public Army AggressorArmy; // { get; private set; }
    public Army DefenderArmy; // { get; private set; }

    private GameObject aggressorGO;
    private GameObject defenderGO;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        OverworldEventBus<InitiateCombat>.OnEvent += InitiateCombat;
        CombatEventBus<UnitKilledEvent>.OnEvent += FinalizeCombat;
    }

    void OnDestroy()
    {
        OverworldEventBus<InitiateCombat>.OnEvent -= InitiateCombat;
        CombatEventBus<UnitKilledEvent>.OnEvent -= FinalizeCombat;
    }

    public void InitiateCombat(InitiateCombat e)
    {
        //TODO: This chain currently directly updates the hero's army in the Overworld. Might want to decouple this for future
        AggressorArmy = e.AggressorArmy;
        DefenderArmy = e.DefenderArmy;

        aggressorGO = e.AggresorObject;
        defenderGO = e.DefenderObject;
        
        HeroMovementManager.Instance.allowInput = false;
        Overworld.SetActive(false);
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive);
    }

    //TODO: Change this so it uses CombatEventBus's CombatEndEvent
    public void FinalizeCombat(UnitKilledEvent e)
    {
        bool armyLost = false;
        foreach (UnitSlot us in e.unit.OwnerArmy._units)
        {
            if (us.identifier != null && us.amount > 0) return;
            armyLost = true;
        }
        if(armyLost)
        {
            SceneManager.UnloadSceneAsync("CombatScene");
            HeroMovementManager.Instance.allowInput = true;
            Overworld.SetActive(true);
            if (e.unit.OwnerArmy == AggressorArmy)
            {
                Destroy(aggressorGO, 0.15f);
            }
            else if (e.unit.OwnerArmy == DefenderArmy)
            {
                Destroy(defenderGO, 0.15f);
            }
            else
            {
                Debug.LogError("Trying to destroy an army that doesn't exist");
            }
        }   
    }
    
}
