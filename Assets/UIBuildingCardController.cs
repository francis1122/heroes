using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class UIBuildingCardController : MonoBehaviour
{
    [SerializeField] GameObject buildingNameTextView;

    [SerializeField] GameObject buildingCost;

    [SerializeField] GameObject buildingAuthCost;

    [SerializeField] GameObject buildingDescription;

    [SerializeField] GameObject purchaseButton;


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
        //  var cansub = buildingObject.CanPurchaseBuilding();


        String buildingName = buildingObject.buildingData.buildingName;


        if (buildingObject.buildingsOwned > 0 && buildingObject.buildingData.repeatablePurchase)
        {
            buildingName = buildingName + "(" + buildingObject.buildingsOwned + ") ";
        }

        if (buildingObject.buildingsOwned > 0 && !buildingObject.buildingData.repeatablePurchase)
        {
            this.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            this.GetComponent<Image>().color = Color.grey;
        }

        if (buildingObject.buildingData.category == BuildingData.BuildingCategory.Event)
        {
            buildingName = buildingName + " time left - " + buildingObject.eventLifeSpanLeft;
        }

        buildingNameTextView.GetComponent<TextMeshProUGUI>().text = buildingName;

        buildingDescription.GetComponent<TextMeshProUGUI>().text = buildingObject.buildingData.buildingDetails;
        buildingAuthCost.GetComponent<TextMeshProUGUI>().text = buildingObject.buildingData.ScaledResourceBundle()
            .GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority).amount.ToString();
        buildingObject.buildingData.costRequirements.resources.Sort((a, b) =>
            a.type.UIPriority.CompareTo(b.type.UIPriority));
        //buildings.Sort((a,b) => a.buildingData.priority.CompareTo(b.buildingData.priority) );
        String resourceCost = buildingObject.buildingData.GetBuildingCostAndRequirementString();

        buildingCost.GetComponent<TextMeshProUGUI>().text = resourceCost;


        if (buildingObject.CanPurchaseBuilding())
        {
            purchaseButton.GetComponent<Image>().color = Color.green;
        }
        else
        {
            purchaseButton.GetComponent<Image>().color = Color.red;
        }

        // should show purchase button or not
        if (!buildingObject.buildingData.repeatablePurchase && buildingObject.timesPurchased > 0)
        {
            purchaseButton.SetActive(false);
        }
        else
        {
            purchaseButton.SetActive(true);
        }

        purchaseButton.GetComponent<Button>().onClick.RemoveAllListeners();
        purchaseButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            buildingObject.PurchaseBuilding(purchaseButton.GetComponent<RectTransform>());
        });
    }
}