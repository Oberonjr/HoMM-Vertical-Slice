using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BuildPanelLogic : MonoBehaviour
{
    public static BuildPanelLogic Instance;
    
    [HideInInspector]public TownData currentTown;
    [HideInInspector]public TownBuildingData selectedBuildingData;
    [HideInInspector]public GameObject townBuildingObject;
    [HideInInspector]public GameObject buildingObjectToDisable;
    
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private TextMeshProUGUI buildingDescription;
    [SerializeField] private GridLayoutGroup resourceGridLayout;
    [SerializeField] private GameObject buildingCostVisuals;
    [SerializeField] private GameObject canBuildButton;
    [SerializeField] private GameObject cannotBuildButton;
    
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        icon.sprite = selectedBuildingData.buildScreenSprite;
        buildingName.text = selectedBuildingData.name;
        buildingDescription.text = selectedBuildingData.description;
        foreach (KeyValuePair<ResourceData.ResourceType, int> cost in selectedBuildingData.cost)
        {
            GameObject costVisual = Instantiate(buildingCostVisuals, resourceGridLayout.transform);
            //costVisual.GetComponent<Sprite>().texture =
            costVisual.GetComponentInChildren<TextMeshProUGUI>().text = cost.Value.ToString();
        }

        if (selectedBuildingData.CanBeBuilt(currentTown)) 
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

    public void Build()
    {
        selectedBuildingData.OnBuild(currentTown);
        townBuildingObject.SetActive(true);
        //SerializeFields do not properly check for null, so do a try for as long as there actually is something there
        try {buildingObjectToDisable.SetActive(false);}catch(Exception e){}
    }
}
