using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverworldUIManager : MonoBehaviour
{
    public static OverworldUIManager Instance;
    
    #region MainCanvas
    [Header("Main Canvas")]
    [SerializeField] private GameObject mainCanvas;
    #endregion
    
    #region EconomyText
    [Header("Economy")]
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text oreText;
    [SerializeField] private TMP_Text crystalText;
    #endregion
    
    #region Calendar
    [Header("Calendar")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text weekText;
    [SerializeField] private TMP_Text monthText;
    #endregion
    
    #region Town
    [HideInInspector] public TownData currentTown;
    private BuildPanelLogic BuildPanel;
    [Header("Building Panel")]
    #endregion
    
    #region StatusImages
    [Header("Status Bar Image Prefabs")]
    public Sprite CanBeBuiltBar;
    public Sprite CanNotBeBuiltBar;
    public Sprite HasBeenBuiltBar;
    #endregion
    
    private Economy playerEconomy;
    private Calendar _calendar;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void OnEnable()
    {
        OverworldEventBus<NewDay>.OnEvent += UpdateTime;
        OverworldEventBus<OpenBuildScreen>.OnEvent += EnableBuildScreen;
        OverworldEventBus<OpenTownScreen>.OnEvent += OpenTownScreen;
    }

    void OnDisable()
    {
        OverworldEventBus<NewDay>.OnEvent -= UpdateTime;
        OverworldEventBus<OpenBuildScreen>.OnEvent -= EnableBuildScreen;
        OverworldEventBus<OpenTownScreen>.OnEvent -= OpenTownScreen;
    }
    void Start()
    {
        playerEconomy = OverworldTurnManager.Instance.ActivePlayer.Kingdom.Economy;
        _calendar = Calendar.Instance;
    }
    
    void Update()
    {
        goldText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Gold].ToString();
        woodText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Wood].ToString();
        oreText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Ore].ToString();
        crystalText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Crystal].ToString();
    }

    void OpenTownScreen(OpenTownScreen e)
    {
        GameObject townScreen = Instantiate(e.town.faction.factionTownUIScreen, mainCanvas.transform);
        BuildPanel = townScreen.GetComponent<TownScreenLogic>().buildPanel;
    }

    void UpdateTime(NewDay e)
    {
        dayText.text = "Day: " + _calendar.Day;
        weekText.text = "Week: " + _calendar.Week;
        monthText.text = "Month: " + _calendar.Month;
    }

    void EnableBuildScreen(OpenBuildScreen e)
    {
        BuildPanel.selectedBuildingData = e.buildingData;
        BuildPanel.townBuildingObject = e.buildingToBuild;
        BuildPanel.buildingObjectToDisable = e.buildingToReplace;
        BuildPanel.gameObject.SetActive(true);
    }
}
