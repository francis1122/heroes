
using System;
using System.Collections.Generic;
using Data;
using GameObjects;
using UnityEngine;
using UnityEngine.UIElements;

public class PopulationPanelController : MonoBehaviour
{
    // Start is called before the first frame update
    VisualElement root;

    
    [SerializeField]
    VisualTreeAsset populationGroupTemplate;

    
    [SerializeField]
    VisualTreeAsset populationTemplate;

    public String rootPanel;

    public BuildingData.BuildingCategory category = BuildingData.BuildingCategory.Building;
    
    private GroupBox populationPanel;
    
    private List<GroupBox> buildingGroups = new List<GroupBox>();

    private List<GroupBox> buildingBoxes = new List<GroupBox>();

    void Start()
    {
        UIDocument menu = GetComponent<UIDocument>();
        root = menu.rootVisualElement;
        populationPanel = root.Q<GroupBox>(rootPanel);
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

            if (count % 2 == 0)
            {
                TemplateContainer buildingGroup = populationGroupTemplate.Instantiate();
                currentGroup = buildingGroup.Q<GroupBox>("population-container-group");
                populationPanel.Add(currentGroup);
                buildingGroups.Add(currentGroup);
            }


            TemplateContainer buildingBox = populationTemplate.Instantiate();
            
            buildingBox.Q<Label>("population_name_label").text = buildingObject.buildingData.buildingName;
            buildingBox.Q<Label>("population_details_label").text = buildingObject.buildingData.buildingDetails;
            String resourceCost = buildingObject.buildingData.ScaledResourceBundle()
                .GetStringDisplay();
            resourceCost += "\n";
                //var test = buildingObject.buildingData.ScaledResourceBundle().GetPopulationRecruitAvailableStringDisplay() +
                 //           " - ";

            buildingBox.Q<Label>("population_cost_label").text = resourceCost;
            buildingBox.Q<Label>("population_limit_label").text = buildingObject.buildingData.GetBuildingPopulationGainMax();
            
            // should show purchase button or not
            if(!buildingObject.buildingData.repeatablePurchase && buildingObject.timesPurchased > 0)
            {
                buildingBox.Q<Button>("population_purchase_button").visible = false;
            }
            else
            {
                buildingBox.Q<Button>("population_purchase_button").visible = true;
            }
            
            buildingBox.Q<Button>("population_purchase_button").RegisterCallback<ClickEvent>((evt =>
            {
                buildingObject.PurchaseBuilding();
            }));
            currentGroup?.Add(buildingBox.Q<GroupBox>("population_box_group"));
            buildingBoxes.Add(buildingBox.Q<GroupBox>("population_box_group"));
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
        
        populationPanel.Clear();
    }
    public void UpdateUI()
    {
        ClearBoxes();
        CreateBuildingBoxes();

    }
}