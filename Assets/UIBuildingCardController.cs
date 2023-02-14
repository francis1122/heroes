using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using GameObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UIBuildingCardController : MonoBehaviour
{
    
    
    [SerializeField]
    GameObject buildingName;
    
    [SerializeField]
    GameObject buildingCost;
    
    [SerializeField]
    GameObject buildingAuthCost;
    
    [SerializeField]
    GameObject buildingDescription;
    
    [SerializeField]
    GameObject purchaseButton;
    

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateUIWithBuilding(BuildingObject buildingObject)
    {
        // set values
        //amount.GetComponent<TextMeshProUGUI>().text = data.type.resourceName + " " + data.amount;
        buildingName.GetComponent<TextMeshProUGUI>().text = buildingObject.buildingData.buildingName;
        buildingDescription.GetComponent<TextMeshProUGUI>().text = buildingObject.buildingData.buildingDetails;
        buildingAuthCost.GetComponent<TextMeshProUGUI>().text = buildingObject.buildingData.ScaledResourceBundle()
            .GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority).amount.ToString();
        String resourceCost = buildingObject.buildingData.GetBuildingCostAndRequirementString();

        buildingCost.GetComponent<TextMeshProUGUI>().text = resourceCost;
            
        // should show purchase button or not
        if(!buildingObject.buildingData.repeatablePurchase && buildingObject.timesPurchased > 0)
        {
            purchaseButton.SetActive(false);
        }
        else
        {
            purchaseButton.SetActive(true);
        }

        purchaseButton.GetComponent<Button>().onClick.RemoveAllListeners();
        purchaseButton.GetComponent<Button>().onClick.AddListener(()=>
        {
            buildingObject.PurchaseBuilding();
        });

        // buildingBox.Q<Button>("building_purchase_button").clickable = null;
        // buildingBox.Q<Button>("building_purchase_button").clickable =
        //     new Clickable(buildingObject.PurchaseBuilding); 
        // //buildingObject.PurchaseBuilding;
        //     
        // //currentGroup?.Add(buildingBox.Q<GroupBox>("building_box_group"));
        // currentGroup?.Add(buildingBox);
        // count++;
    }
}
