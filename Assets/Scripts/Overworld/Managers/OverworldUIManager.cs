using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverworldUIManager : MonoBehaviour
{
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
    
    private Economy playerEconomy;
    private Calendar _calendar;

    void OnEnable()
    {
        OverworldEventBus<NewDay>.OnEvent += UpdateTime;
    }

    void OnDisable()
    {
        OverworldEventBus<NewDay>.OnEvent -= UpdateTime;
    }
    void Start()
    {
        playerEconomy = OverworldTurnManager.Instance.ActivePlayer.Kingdom.Economy;
        _calendar = Calendar.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Gold].ToString();
        woodText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Wood].ToString();
        oreText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Ore].ToString();
        crystalText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Crystal].ToString();
    }

    void UpdateTime(NewDay e)
    {
        dayText.text = "Day: " + _calendar.Day;
        weekText.text = "Week: " + _calendar.Week;
        monthText.text = "Month: " + _calendar.Month;
    }
}
