using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonLogic : MonoBehaviour
{
    private TownData currentTown;
    
    #region TownBuildingData
    [Header("TownBuildingData")]
    [SerializeField] private TownBuildingData townBuildingData;
    [SerializeField] private TownBuildingData newDataToEnable;
    #endregion
    
    #region TownPanelUIElements
    [Header("Town Images to change")]
    [SerializeField] private GameObject townBuildingObject;
    [SerializeField] private GameObject buildingToBeReplaced;
    #endregion
    
    #region ButtonVisualComponents
    [Header("Visual Components to be updated")]
    [SerializeField] private Image buildingButtonSprite;
    [SerializeField] private Image buildStatusSprite;
    [SerializeField] private TMP_Text buildingNameTextField;
    #endregion
    
    private void OnEnable()
    {
        //TODO: Initialize the data here properly in order to set the correct graphic depending on built status
        
        if (currentTown.builtBuildings.Contains(townBuildingData) && townBuildingData.buildingToEnable == null)
        {
            buildStatusSprite.sprite = OverworldUIManager.Instance.HasBeenBuiltBar;
        }
        else if (townBuildingData.CanBeBuilt(currentTown))
        {
            buildStatusSprite.sprite = OverworldUIManager.Instance.CanBeBuiltBar;
        }
        else
        {
            buildStatusSprite.sprite = OverworldUIManager.Instance.CanNotBeBuiltBar;
        }
        
        buildingButtonSprite.sprite = townBuildingData.buildScreenSprite;
        buildingNameTextField.text = townBuildingData.name;
    }

    public void OpenBuildPanel()
    {
        OverworldEventBus<OpenBuildScreen>.Publish(new OpenBuildScreen(townBuildingData, townBuildingObject, buildingToBeReplaced));
    }
}
