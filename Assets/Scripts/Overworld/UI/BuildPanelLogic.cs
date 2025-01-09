using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BuildPanelLogic : MonoBehaviour
{
    
    [HideInInspector]public TownData currentTown;
    [HideInInspector]public TownBuildingData selectedBuildingData;
    [HideInInspector]public GameObject townBuildingObject;
    [HideInInspector]public GameObject buildingObjectToDisable;
    
    #region PanelUI
    [Header("Panel UI Elements")]
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private TextMeshProUGUI buildingDescription;
    [SerializeField] private GridLayoutGroup resourceGridLayout;
    [SerializeField] private GameObject buildingCostVisuals;
    [SerializeField] private GameObject canBuildButton;
    [SerializeField] private GameObject cannotBuildButton;
    #endregion
    
    
    
    private List<GameObject> costVisuals = new List<GameObject>();
    private void OnEnable()
    {
        currentTown = OverworldUIManager.Instance.currentTown;
        icon.sprite = selectedBuildingData.buildScreenSprite;
        buildingName.text = selectedBuildingData.name;
        buildingDescription.text = selectedBuildingData.description;
        foreach (KeyValuePair<ResourceData.ResourceType, int> cost in selectedBuildingData.cost)
        {
            GameObject costVisual = Instantiate(buildingCostVisuals, resourceGridLayout.transform);
            costVisuals.Add(costVisual);
            //costVisual.GetComponent<Sprite>().texture =
            costVisual.GetComponentInChildren<TextMeshProUGUI>().text = cost.Value.ToString();
        }

        if (selectedBuildingData.CanBeBuilt(currentTown) && currentTown.CanBuild) 
        {
            canBuildButton.SetActive(true);
            cannotBuildButton.SetActive(false);
        }
        else
        {
            cannotBuildButton.SetActive(true);
            canBuildButton.SetActive(false);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject costVisual in costVisuals)
        {
            Destroy(costVisual);    
        }
        costVisuals.Clear();
    }
    
    public void Build()
    {
        selectedBuildingData.OnBuild(currentTown);
        townBuildingObject.SetActive(true);
        //SerializeFields do not properly check for null, so do a try for as long as there actually is something there
        try {buildingObjectToDisable.SetActive(false);}catch(Exception e){}
    }
}
