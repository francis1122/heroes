using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrestigeText : MonoBehaviour
{
    [SerializeField] private GameObject prestigeLabel;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
        EventManager.StartListening(EventManager.EVENT_END_TURN, UpdateUI);
    }


    void UpdateUI()
    {

        prestigeLabel.GetComponent<TextMeshProUGUI>().text = "Prestige: " + GameCenter.instance.prestigeScore;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
