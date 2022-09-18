
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    private GroupBox buildingPanel;
    
    private List<GroupBox> buildingGroups = new List<GroupBox>();

    private List<GroupBox> buildingBoxes = new List<GroupBox>();

    void Start()
    {
        UIDocument menu = GetComponent<UIDocument>();
        root = menu.rootVisualElement;
        buildingPanel = root.Q<GroupBox>(rootPanel);
        CreateBuildingBoxes();
        
        EventManager.StartListening(EventManager.EVENT_END_TURN, UpdateUI );
        EventManager.StartListening(EventManager.RESOURCES_CHANGED, UpdateUI );
        EventManager.StartListening(EventManager.BUILDING_CHANGED, UpdateUI );
        
    }

    public void SortBuildingsPriority()
    {
        List<BuildingObject> buildings = GameCenter.instance.purchasableBuildings;
        buildings.Sort((a,b) => a.buildingData.priority.CompareTo(b.buildingData.priority) );
        //tests
    }
    
    public void CreateBuildingBoxes()
    {
        SortBuildingsPriority();
        List<BuildingObject> buildings = GameCenter.instance.purchasableBuildings;
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


            TemplateContainer buildingBox = buildingTemplate.Instantiate();
            
            buildingBox.Q<Label>("building_name_label").text = buildingObject.buildingData.buildingName;
            buildingBox.Q<Label>("building_details_label").text = buildingObject.buildingData.buildingDetails;
            buildingBox.Q<Label>("building_cost_label").text = buildingObject.buildingData.costRequirements.GetStringDisplay();
            
            // should show purchase button or not
            if(!buildingObject.buildingData.repeatablePurchase && buildingObject.timesPurchased > 0)
            {
                buildingBox.Q<Button>("building_purchase_button").visible = false;
            }
            else
            {
                buildingBox.Q<Button>("building_purchase_button").visible = true;
            }
            
            buildingBox.Q<Button>("building_purchase_button").RegisterCallback<ClickEvent>((evt =>
            {
                buildingObject.PurchaseBuilding();
            }));
            currentGroup?.Add(buildingBox.Q<GroupBox>("building_box_group"));
            buildingBoxes.Add(buildingBox.Q<GroupBox>("building_box_group"));
            count++;
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
        buildingBoxes.Clear();
        buildingGroups.Clear();
        
        buildingPanel.Clear();
    }
    public void UpdateUI()
    {
        ClearBoxes();
        CreateBuildingBoxes();

    }
}