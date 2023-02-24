
using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using GameObjects;
using UnityEngine;
using UnityEngine.UIElements;
public class BuildingPanelController : MonoBehaviour
{
    // Start is called before the first frame update
    VisualElement root;

    
    [SerializeField]
    VisualTreeAsset buildingGroupTemplate;

    
    [SerializeField]
    VisualTreeAsset buildingTemplate;

    public String rootPanel;

    public BuildingData.BuildingCategory category = BuildingData.BuildingCategory.Building;
    
    // holder of all stuff\
        
    [SerializeField]
    public GroupBox buildingPanel;
    
    
    // holds a row of boxes
        
    [SerializeField]
    public List<GroupBox> buildingGroups = new List<GroupBox>();

    // actual building cell
        
    [SerializeField]
    public Stack<GroupBox> activeBuildingBoxes = new Stack<GroupBox>();
        
    [SerializeField]
    public Stack<GroupBox> poolBuildingBoxes = new Stack<GroupBox>();

    void Start()
    {
        UIDocument menu = GetComponent<UIDocument>();
        root = menu.rootVisualElement;
        buildingPanel = root.Q<GroupBox>(rootPanel);

        for (int i = 0; i < 30; i++)
        {
            TemplateContainer buildingBox = buildingTemplate.Instantiate();
            poolBuildingBoxes.Push(buildingBox.Q<GroupBox>("building_box_group"));
        }

        CreateBuildingBoxes();
        
        EventManager.StartListening(EventManager.EVENT_END_TURN, UpdateUI );
        EventManager.StartListening(EventManager.RESOURCES_CHANGED, UpdateUI );
        EventManager.StartListening(EventManager.BUILDING_CHANGED, UpdateUI );
        
    }

    public void SortBuildingsPriority()
    {
        List<BuildingObject> buildings = GameCenter.instance.playerBuildings;
        //buildings.Sort((a,b) => a.buildingData.priority.CompareTo(b.buildingData.priority) );
        //tests
    }
    
    public void CreateBuildingBoxes()
    {
        SortBuildingsPriority();
        List<BuildingObject> buildings = GameCenter.instance.playerBuildings;   
        List<BuildingObject> onlyCategory = buildings.FindAll(e => e.buildingData.category == category);
        GroupBox currentGroup = null;
        int count = 0;
        foreach (BuildingObject buildingObject in onlyCategory)
        {

            if (count % 4 == 0)
            {
                TemplateContainer buildingGroup = buildingGroupTemplate.Instantiate();
                currentGroup = buildingGroup.Q<GroupBox>("building-container-group");
                buildingPanel.Add(currentGroup);
                buildingGroups.Add(currentGroup);
            }


            GroupBox buildingBox = poolBuildingBoxes.Pop();
            
            activeBuildingBoxes.Push(buildingBox);
                //buildingTemplate.Instantiate();
                
            // clear values
            
            
            
            // set values
            
            buildingBox.Q<Label>("building_name_label").text = buildingObject.buildingData.buildingName;
            buildingBox.Q<Label>("building_details_label").text = buildingObject.buildingData.buildingDetails;
            buildingBox.Q<Label>("building_auth_label").text = buildingObject.buildingData.ScaledResourceBundle()
                .GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority).amount.ToString();
            String resourceCost = buildingObject.buildingData.GetBuildingCostAndRequirementString();

            buildingBox.Q<Label>("building_cost_label").text = resourceCost;
            
            // should show purchase button or not
            if(!buildingObject.buildingData.repeatablePurchase && buildingObject.timesPurchased > 0)
            {
                buildingBox.Q<Button>("building_purchase_button").visible = false;
            }
            else
            {
                buildingBox.Q<Button>("building_purchase_button").visible = true;
            }
            
            buildingBox.Q<Button>("building_purchase_button").clickable = null;
            // buildingBox.Q<Button>("building_purchase_button").clickable =
            //     new Clickable(buildingObject.PurchaseBuilding); 
                //buildingObject.PurchaseBuilding;
            
            //currentGroup?.Add(buildingBox.Q<GroupBox>("building_box_group"));
            currentGroup?.Add(buildingBox);
            count++;
            //poolBuildingBoxes.RemoveAt(poolBuildingBoxes.Count - 1);
        }

        /*
        //ui_start_button
        playerMoneyLabel = root.Q<Label>("player-money-label");
        currentTurnLabel = root.Q<Label>("turn-number-label");
        endTurnButton = root.Q<Button>("end-turn-button");
        
        
        
        endTurnButton.RegisterCallback<ClickEvent>((evt =>
        {
            GameCenter.instance.EndTurn();
        EndTurn}));*/
    }

    public void ClearBoxes()
    {

      //  foreach (var activeBuildingBox in activeBuildingBoxes)


      while (activeBuildingBoxes.Count > 0)
      {
          var test = activeBuildingBoxes.Pop();

          poolBuildingBoxes.Push(test);
          //test?.Q<GroupBox>("building_box_group").Remove();
      }

      //  }
        //activeBuildingBoxes.Clear();
        buildingGroups.Clear();
        
        // this removes all elemenents from view
        buildingPanel.Clear();
    }
    public void UpdateUI()
    {
        ClearBoxes();
        CreateBuildingBoxes();

    }
}