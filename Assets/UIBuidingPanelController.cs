using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using GameObjects;
using UnityEngine;
using UnityEngine.UIElements;

public class UIBuidingPanelController : MonoBehaviour
{
    
    [SerializeField] GameObject buildingTemplate;
    public BuildingData.BuildingCategory category = BuildingData.BuildingCategory.Building;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
        EventManager.StartListening(EventManager.EVENT_END_TURN, UpdateUI );
        EventManager.StartListening(EventManager.RESOURCES_CHANGED, UpdateUI );
        EventManager.StartListening(EventManager.BUILDING_CHANGED, UpdateUI );
    }


    public void UpdateUI()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        
        List<BuildingObject> buildings = GameCenter.instance.playerBuildings;   
        List<BuildingObject> onlyCategory = buildings.FindAll(e => e.buildingData.category == category);
        onlyCategory.Sort((a,b) => a.buildingData.priority.CompareTo(b.buildingData.priority) );
        GroupBox currentGroup = null;
        int count = 0;
        foreach (BuildingObject buildingObject in onlyCategory)
        {
            GameObject newBuildingCard = Instantiate(buildingTemplate, this.transform);
            //Vector3 newPos = newBuildingCard.transform.localPosition;
            // newPos.x = count%4 * 350;
            // newPos.y = count/4 * -350;
            // newBuildingCard.transform.localPosition = newPos;
            count++;
            if (count % 4 == 0)
            {
                
                // TemplateContainer buildingGroup = buildingGroupTemplate.Instantiate();
                // currentGroup = buildingGroup.Q<GroupBox>("building-container-group");
                // buildingPanel.Add(currentGroup);
                // buildingGroups.Add(currentGroup);
            }
            //GroupBox buildingBox = poolBuildingBoxes.Pop();
            //activeBuildingBoxes.Push(buildingBox);
                //buildingTemplate.Instantiate();
                
            // clear values
            
            
            
            // set values
            
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
