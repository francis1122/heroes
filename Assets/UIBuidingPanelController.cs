using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using GameObjects;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIBuidingPanelController : MonoBehaviour
{
    
    public Stack<GameObject> activeBuildingBoxes = new Stack<GameObject>();
    
    public Stack<GameObject> poolBuildingBoxes = new Stack<GameObject>();

    
    [SerializeField] GameObject buildingTemplate;
    public BuildingData.BuildingCategory category = BuildingData.BuildingCategory.Building;
    
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < 50; i++)
        {
            GameObject newBuildingCard = Instantiate(buildingTemplate, this.transform);
            newBuildingCard.SetActive(false);
            poolBuildingBoxes.Push(newBuildingCard);
        }
        
        UpdateUI();
        EventManager.StartListening(EventManager.EVENT_END_TURN, UpdateUI );
        EventManager.StartListening(EventManager.RESOURCES_CHANGED, UpdateUI );
        EventManager.StartListening(EventManager.BUILDING_CHANGED, UpdateUI );
    }


    public void UpdateUI()
    {
        
        // remove all active
        while (activeBuildingBoxes.Count > 0)
        {
            
            var activeBuilding = activeBuildingBoxes.Pop();
            activeBuilding.SetActive(false);
           // activeBuilding.transform.parent = null;
            //test.transform.parent = this.transform;
            poolBuildingBoxes.Push(activeBuilding);
        }


        List<BuildingObject> buildings = GameCenter.instance.playerBuildings;   
        List<BuildingObject> onlyCategory = buildings.FindAll(e => e.buildingData.category == category);
        onlyCategory.Sort((a,b) => a.buildingData.priority.CompareTo(b.buildingData.priority) );
        onlyCategory.Reverse();
        foreach (BuildingObject buildingObject in onlyCategory)
        {
            GameObject newBuildingCard = poolBuildingBoxes.Pop();
            activeBuildingBoxes.Push(newBuildingCard);
            newBuildingCard.SetActive(true);
          //  newBuildingCard.transform.parent = this.transform;
            

            newBuildingCard.GetComponent<UIBuildingCardController>().UpdateUIWithBuilding(buildingObject);
            
            //
            // buildingBox.Q<Label>("building_name_label").text = buildingObject.buildingData.buildingName;
            // buildingBox.Q<Label>("building_details_label").text = buildingObject.buildingData.buildingDetails;
            // buildingBox.Q<Label>("building_auth_label").text = buildingObject.buildingData.ScaledResourceBundle()
            //     .GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority).amount.ToString();
            // String resourceCost = buildingObject.buildingData.GetBuildingCostAndRequirementString();
            //
            // buildingBox.Q<Label>("building_cost_label").text = resourceCost;
            //
            // // should show purchase button or not
            // if(!buildingObject.buildingData.repeatablePurchase && buildingObject.timesPurchased > 0)
            // {
            //     buildingBox.Q<Button>("building_purchase_button").visible = false;
            // }
            // else
            // {
            //     buildingBox.Q<Button>("building_purchase_button").visible = true;
            // }
            //
            // buildingBox.Q<Button>("building_purchase_button").clickable = null;
            // buildingBox.Q<Button>("building_purchase_button").clickable =
            //     new Clickable(buildingObject.PurchaseBuilding); 
            //     //buildingObject.PurchaseBuilding;
            //
            // //currentGroup?.Add(buildingBox.Q<GroupBox>("building_box_group"));
            // currentGroup?.Add(buildingBox);
            // count++;
            //poolBuildingBoxes.RemoveAt(poolBuildingBoxes.Count - 1);
        }
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
