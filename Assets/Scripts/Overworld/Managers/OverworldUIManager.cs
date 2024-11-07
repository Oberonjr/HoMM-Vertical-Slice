using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverworldUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text oreText;
    [SerializeField] private TMP_Text crystalText;
    
    private Economy playerEconomy;
    
    // Start is called before the first frame update
    void Start()
    {
        playerEconomy = OverworldTurnManager.Instance.ActivePlayer.Kingdom.Economy;
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Gold].ToString();
        woodText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Wood].ToString();
        oreText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Ore].ToString();
        crystalText.text = playerEconomy.ResourceAmount[ResourceData.ResourceType.Crystal].ToString();
    }
}
