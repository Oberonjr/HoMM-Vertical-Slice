using System.Collections;
using System.Collections.Generic;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GridTracker : MonoBehaviour
{
    [SerializeField] private SceneField OverworldScene;
    [SerializeField] private SceneField CombatScene;
    private SceneField currentSceneField;
    
    public Dictionary<Vector2, Node> OverworldGrid = new Dictionary<Vector2, Node>();
    public Dictionary<Vector2, Node> CombatGrid = new Dictionary<Vector2, Node>();
    public Dictionary<Vector2, Node> CurrentGrid = null;

    [HideInInspector] public Tilemap currentTilemap;
    
    public static GridTracker Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

        if (currentTilemap == null)
        {
            currentTilemap = FindObjectOfType<Tilemap>();
        }
        GeneralEventBus<GenerateGridEvent>.OnEvent += UpdateGridValues;
        SceneManager.sceneLoaded += UpdatecurrentSceneField;
        SceneManager.sceneUnloaded += UpdatecurrentSceneField;
    }

    void Start()
    {
    }

    void OnDestroy()
    {
        GeneralEventBus<GenerateGridEvent>.OnEvent -= UpdateGridValues;
        SceneManager.sceneLoaded -= UpdatecurrentSceneField;
        SceneManager.sceneUnloaded -= UpdatecurrentSceneField;
    }

    //Used for loading in scenes
    void UpdatecurrentSceneField(Scene argo0, LoadSceneMode arg1)
    {
        if (argo0.name == OverworldScene.Name)
        {
            currentSceneField = OverworldScene;
            if(!(OverworldGrid.Count <= 0))CurrentGrid = OverworldGrid;
        }
        else if (argo0.name == CombatScene.Name)
        {
            currentSceneField = CombatScene;
            if(!(CombatGrid.Count <= 0))CurrentGrid = CombatGrid;
        }
        currentTilemap = FindObjectOfType<Tilemap>();
    }

    //Used for unloading Combat scene
    void UpdatecurrentSceneField(Scene arg0)
    {
        if (arg0.name == CombatScene.Name)
        {
            currentSceneField = OverworldScene;
            if(!(OverworldGrid.Count <= 0))CurrentGrid = OverworldGrid;
            currentTilemap = FindObjectOfType<Tilemap>();
            CombatGrid = null;
        }
    }

    void UpdateGridValues(GenerateGridEvent e)
    {
        if (currentSceneField == OverworldScene)
        {
            OverworldGrid = e.grid;
            if (CurrentGrid == null || CurrentGrid.Count == 0)
            {
                CurrentGrid = OverworldGrid;
            }
        }
        else if (currentSceneField == CombatScene)
        {
            CombatGrid = e.grid;
            CurrentGrid = CombatGrid; //TODO: Find a better way to make this switch
        }
        else
        {
            Debug.LogError("Trying to generate a grid while not in Overworld nor Combat Scene.");
        }
    }
}