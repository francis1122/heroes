using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEndOfTurnText : MonoBehaviour
{
    [SerializeField] private GameObject currentTurnLabel;
    [SerializeField] private GameObject endTurnButton;
    
    // Start is called before the first frame update
    void Start()
    {
        endTurnButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameCenter.instance.EndTurn();
        });
        UpdateUI();
        EventManager.StartListening(EventManager.EVENT_END_TURN, UpdateUI);
    }


    void UpdateUI()
    {
        
        currentTurnLabel.GetComponent<TextMeshProUGUI>().text = "season " + (GameCenter.instance.currentTurn % GameCenter.instance.seasonsInAYear) + " year " + GameCenter.instance.currentTurn / 4;
        //endTurnButton.GetComponentInChildren<TextMeshProUGUI>().text = ""
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
