using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIResourceUnitController : MonoBehaviour
{
    [SerializeField]
    GameObject thumbnail;
    
    [SerializeField]
    GameObject amount;
    
    [SerializeField]
    GameObject futureGain;
    
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(EventManager.EVENT_BUILDING_ADDED, UpdateUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        
    }

    public void UpdateUIWithResource(ResourceData data, ResourceData buffer)
    {
        amount.GetComponent<TextMeshProUGUI>().richText = true;
        var color = "#999999";
        var sign = "";
        if (buffer.amount > 0)
        {
            sign = "+";
            color = "#33DD33";
        }else if (buffer.amount < 0)
        {
            color = "#FF8888";
        }
        amount.GetComponent<TextMeshProUGUI>().text =
            data.type.resourceName +": " + data.amount + " <"+color+">" + sign+buffer.amount + "</color>";
                                                      //<#00ff00>Green text</color> " + ;
        //futureGain.GetComponent<TextMeshProUGUI>().text = buffer.amount + "";
        //data.amount
        //data.type.resourceName
    }
    
    public void UpdateUIWithResource(float data, int dataMax, float buffer)
    {
        amount.GetComponent<TextMeshProUGUI>().text = String.Format( format: "Pop {0:0.##} / {1:0.##}",   data, dataMax );
        futureGain.GetComponent<TextMeshProUGUI>().text = $"{buffer:0.##}";
        //<#00ff00>Green text</color> " + ;
        //futureGain.GetComponent<TextMeshProUGUI>().text = buffer.amount + "";
        //data.amount
        //data.type.resourceName
    }
    
    public void UpdateUIWithResourceLimit(ResourceData data, ResourceData limit, ResourceData buffer)
    {
        amount.GetComponent<TextMeshProUGUI>().text = data.type.resourceName + " " + data.amount + "/" + limit.amount;
        futureGain.GetComponent<TextMeshProUGUI>().text = buffer.amount + "";
        //data.amount
        //data.type.resourceName
    }
    
}
