using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitButtonLogic : MonoBehaviour
{
    public CreatureDwellingInfo dwellingInfo;


    private void OnEnable()
    {
        dwellingInfo.StationedAmont = dwellingInfo.ProducedUnit.Growth;
    }

    public void Recruit()
    {
        OverworldEventBus<OpenRecruitScreen>.Publish(new OpenRecruitScreen(dwellingInfo));
    }
}
