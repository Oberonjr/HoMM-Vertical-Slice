using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonLogic : MonoBehaviour
{
    private TownData currentTown;
    
    [SerializeField] private TownBuildingData townBuildingData;
    [SerializeField] private GameObject townBuildingObject;
    [SerializeField] private GameObject buildingToBeReplaced;
    
    [SerializeField] private Image buildingButtonSprite;
    [SerializeField] private Image buildStatusSprite;
    [SerializeField] private TMP_Text buildingNameText;

    private void OnEnable()
    {
        //TODO: Initialize the data here properly in order to set the correct graphic depending on built status
        /*
        if (currentTown.builtBuildings.Contains(townBuildingData))
        {
            //buildStatusSprite.sprite = ...alreadyBuiltImage
        }
        else if (townBuildingData.CanBeBuilt())
        {
            //buildStatusSprite.sprite = ...canBuildImage
        }
        else
        {
            //buildStatusSprite.sprite = ...cantBuildImage
        }
        */
        buildingButtonSprite.sprite = townBuildingData.buildScreenSprite;
        buildingNameText.text = townBuildingData.name;
    }

    public void OpenBuildPanel()
    {
        OverworldEventBus<OpenBuildScreen>.Publish(new OpenBuildScreen(townBuildingData, townBuildingObject, buildingToBeReplaced));
    }
}
