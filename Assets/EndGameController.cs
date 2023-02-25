using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    [SerializeField] GameObject prestigeText;
    [SerializeField] GameObject currentUpgrades;

    [SerializeField] GameObject upgradeGold;
    [SerializeField] GameObject upgradeLumber;
    [SerializeField] GameObject upgradeLand;
    [SerializeField] GameObject upgradeOre;

    [SerializeField] GameObject newEmpireButton;

    // Start is called before the first frame update
    void Start()
    {
        // we can use this to animate
        GameCenter.instance.totalPrestigeScore += GameCenter.instance.prestigeScore;

        UpdateUI();

        //
        //
        //

        upgradeGold.GetComponentInChildren<TextMeshProUGUI>().text =
            "100 extra gold when you start your next empire \n costs 100 Prestige";
        upgradeGold.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (GameCenter.instance.totalPrestigeScore >= 100)
            {
                GameCenter.instance.playerBoostResources.AddResourceData(GameCenter.instance.resourceOrganizer
                    .CreateResourceData(100, ResourceType.LinkType.Gold));
                GameCenter.instance.totalPrestigeScore -= 100;
                UpdateUI();
            }
        });

        upgradeLumber.GetComponentInChildren<TextMeshProUGUI>().text =
            "25 extra lumber when you start your next empire \n costs 100 Prestige";
        upgradeLumber.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (GameCenter.instance.totalPrestigeScore >= 100)
            {
                GameCenter.instance.playerBoostResources.AddResourceData(GameCenter.instance.resourceOrganizer
                    .CreateResourceData(25, ResourceType.LinkType.Lumber));
                GameCenter.instance.totalPrestigeScore -= 100;
                UpdateUI();
            }
        });
        
        upgradeOre.GetComponentInChildren<TextMeshProUGUI>().text =
            "25 extra ore when you start your next empire \n costs 100 Prestige";
        upgradeOre.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (GameCenter.instance.totalPrestigeScore >= 100)
            {
                GameCenter.instance.playerBoostResources.AddResourceData(GameCenter.instance.resourceOrganizer
                    .CreateResourceData(25, ResourceType.LinkType.Ore));
                GameCenter.instance.totalPrestigeScore -= 100;
                UpdateUI();
            }
        });
        
        upgradeLand.GetComponentInChildren<TextMeshProUGUI>().text =
            "15 extra land when you start your next empire \n costs 100 Prestige";
        upgradeLand.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (GameCenter.instance.totalPrestigeScore >= 100)
            {
                GameCenter.instance.playerBoostResources.AddResourceData(GameCenter.instance.resourceOrganizer
                    .CreateResourceData(15, ResourceType.LinkType.BasicLand));
                GameCenter.instance.totalPrestigeScore -= 100;
                UpdateUI();
            }
        });


        //
        //
        //


        newEmpireButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameCenter.instance.ResetEmpire();
            SceneManager.LoadScene("GameScene");
        });
    }

    void UpdateUI()
    {
        prestigeText.GetComponent<TextMeshProUGUI>().text = "Run Gained: " + GameCenter.instance.prestigeScore
                                                                           + "\n Total Prestige: " +
                                                                           GameCenter.instance.totalPrestigeScore;

        currentUpgrades.GetComponent<TextMeshProUGUI>().text = "Starting boosts: " +
                                                               GameCenter.instance.playerBoostResources
                                                                   .GetStringDisplay();
    }

    // Update is called once per frame
    void Update()
    {
    }
}