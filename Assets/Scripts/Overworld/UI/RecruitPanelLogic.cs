using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RecruitPanelLogic : MonoBehaviour
{
    [SerializeField] private GameObject recruitPanel;
    [SerializeField] private Image icon;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text currentBuyText;
    [SerializeField] private TMP_Text availableText;

    private Player currentPlayer;
    private Unit currentUnit;
    
    void Awake()
    {
        OverworldEventBus<OpenRecruitScreen>.OnEvent += InitializeScreen;
    }

    private void OnDestroy()
    {
        OverworldEventBus<OpenRecruitScreen>.OnEvent -= InitializeScreen;
    }

    void InitializeScreen(OpenRecruitScreen e)
    {
        recruitPanel.SetActive(true);
        icon.sprite = e.unit.unitStats.icon;
        slider.maxValue = e.amount;
        availableText.text = "Available: " + e.amount;
        
        currentPlayer = OverworldTurnManager.Instance.ActivePlayer;
        currentUnit = e.unit;
    }

    public void AddRecruit()
    {
        slider.value++;
    }

    public void SubstractRecruit()
    {
        slider.value--;
    }

    public void UpdateCurrentBuyText()
    {
        currentBuyText.text = "Currently Buying: " + slider.value;
    }

    //Why is non-UI logic happening on a UI element? Because I'm dead inside and couldn't care less anymore
    public void RecruitUnit()
    {
        if (currentPlayer.Kingdom.Economy.CanSpendResource(ResourceData.ResourceType.Gold,
                Mathf.RoundToInt(slider.value) * currentUnit.unitStats.Cost)) //The check already happens in Spend(), but my sequencing is still currently all wrong
        {
            currentPlayer.Heroes[0].AddUnit(currentUnit, Mathf.RoundToInt(slider.value));
            currentPlayer.Kingdom.Economy.SpendResource(ResourceData.ResourceType.Gold, Mathf.RoundToInt(slider.value) * currentUnit.unitStats.Cost);
        }
        else
        {
            Debug.Log("You do not have enough gold to buy this many units");
        }
    }
}
