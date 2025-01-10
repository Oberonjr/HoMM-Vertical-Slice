using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitButtonLogic : MonoBehaviour
{
    public CreatureDwellingInfo dwellingInfo;

    public void Recruit()
    {
        OverworldEventBus<OpenRecruitScreen>.Publish(new OpenRecruitScreen(dwellingInfo));
    }
}
