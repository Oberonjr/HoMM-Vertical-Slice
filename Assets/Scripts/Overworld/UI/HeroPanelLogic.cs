using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroPanelLogic : MonoBehaviour
{
    [SerializeField] private Image heroImage;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text defenseText;
    [SerializeField] private TMP_Text powerText;
    [SerializeField] private TMP_Text knowledgeText;

    [SerializeField] private Image[] creatureIcons;
    [SerializeField] private TMP_Text[] stackSizeText;
    
    private HeroManager _currentHero;
    
    // Start is called before the first frame update
    void Start()
    {
        GetCurrentHero();
        OverworldEventBus<OnPlayerTurnStart>.OnEvent += GetCurrentHero;
    }

    //Some semblance of non-horrendous code, get the first (and currently only) hero to set all the visuals and see if army logic actually works
    void GetCurrentHero(OnPlayerTurnStart e = null)
    {
        _currentHero = OverworldTurnManager.Instance.ActivePlayer.Heroes[0];
    }

    // All the logic here should be on a function that triggers on an event when you switch between the heroes that a player has. Oh well
    void Update()
    {
        heroImage.sprite = _currentHero.cHeroInfo.Icon;
        attackText.text = _currentHero.cHeroInfo.AttackStat.ToString();
        defenseText.text = _currentHero.cHeroInfo.DefenseStat.ToString();
        powerText.text = _currentHero.cHeroInfo.PowerStat.ToString();
        knowledgeText.text = _currentHero.cHeroInfo.KnowledgeStat.ToString();
        
        //Loop through the static internal unit icons list and cross-refference with the actual data
        //so I can set empty slots to other visual stuff to check that logic works
        for (int i = 0; i < creatureIcons.Length; i++)
        {
            if (_currentHero.Army()[i] != null)
            {
                creatureIcons[i].sprite = _currentHero.Army()[i].unitStats.icon;
                stackSizeText[i].text = _currentHero.Army()[i].stackSize.ToString();
            }
            else
            {
                stackSizeText[i].text = " ";
            }
        }
    }
}
