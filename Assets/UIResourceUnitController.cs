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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUIWithResource(ResourceData data, ResourceData buffer)
    {
        amount.GetComponent<TextMeshProUGUI>().text = data.type.resourceName + " " + data.amount;
        futureGain.GetComponent<TextMeshProUGUI>().text = buffer.amount + "";
        //data.amount
        //data.type.resourceName
    }
    
}
