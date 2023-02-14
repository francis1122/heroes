using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIEndOfTurnText : MonoBehaviour
{

    [SerializeField] private GameObject currentTurnLabel;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
        EventManager.StartListening(EventManager.EVENT_END_TURN, UpdateUI);
    }


    void UpdateUI()
    {
        currentTurnLabel.GetComponent<TextMeshProUGUI>().text = "season " + (GameCenter.instance.currentTurn % GameCenter.instance.seasonsInAYear) + " year " + GameCenter.instance.currentTurn / 4;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
