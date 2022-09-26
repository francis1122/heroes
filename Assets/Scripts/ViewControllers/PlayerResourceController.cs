using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerResourceController : MonoBehaviour
{
    // Start is called before the first frame update
    VisualElement root;

    //private Label playerMoneyLabel;
    private Label currentTurnLabel;
    private Button endTurnButton;
    private Label populationLimitLabel;

    private ListView empireList;
    
    
    [SerializeField]
    VisualTreeAsset resourceTemplate;

    private GroupBox resourceContainer;
    
    void Start()
    {
        UIDocument menu = GetComponent<UIDocument>();
        root = menu.rootVisualElement;
        RegisterUI();
        
        EventManager.StartListening(EventManager.EVENT_END_TURN, UpdateUI );
        EventManager.StartListening(EventManager.RESOURCES_CHANGED, UpdateUI );
        
    }

    public void RegisterUI()
    {
        //ui_start_button
        //playerMoneyLabel = root.Q<Label>("player-money-label");
        currentTurnLabel = root.Q<Label>("turn-number-label");
        endTurnButton = root.Q<Button>("end-turn-button");
        populationLimitLabel = root.Q<Label>("population-limit-text");
        resourceContainer = root.Q<GroupBox>("resource-container");
        //SetupEmpireList();
        
        
        endTurnButton.RegisterCallback<ClickEvent>((evt =>
        {
            GameCenter.instance.EndTurn();
        }));
        UpdateUI();
    }

    public void CreateResourceUnits()
    {
        foreach (var resource in GameCenter.instance.playerResources.resources.FindAll(e=> e.type.resourceCategory is ResourceType.ResourceCategory.Material or ResourceType.ResourceCategory.Unique))
        {
            TemplateContainer buildingBox = resourceTemplate.Instantiate();

            //buildingBox.Q<Label>("resource-thumbnail")
            buildingBox.Q<Label>("resource-owned").text = resource.type.resourceName + ": " + resource.amount.ToString();
            ResourceData bufferResourceData =
                GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceType(resource.type);
            
            buildingBox.Q<Label>("resource-gain").text = bufferResourceData.amount.ToString();
            if (resource.type.UITexture != null)
            {
                buildingBox.Q<UIToolKitImage>("resource-thumbnail").image = resource.type.UITexture;
            }
            resourceContainer.Add(buildingBox.Q<GroupBox>("resource-unit"));
        }
    }

    public void ClearResourceUnits()
    {
        resourceContainer.Clear();
    }

    /*
    public void SetupEmpireList()
    {
        empireList = root.Q<ListView>("empire-list");
        // Create a list of data. In this case, numbers from 1 to 1000.
        const int itemCount = 100;
        var items = new List<string>(itemCount);
        for (int i = 1; i <= itemCount; i++)
        {
            items.Add(i.ToString());
        }



        Debug.Log("empire List22");
        // The "makeItem" function is called when the
        // ListView needs more items to render.
        Func<VisualElement> makeItem = () =>
        {
            return new Label();
        };
        
        // As the user scrolls through the list, the ListView object
        // recycles elements created by the "makeItem" function,
        // and invoke the "bindItem" callback to associate
        // the element with the matching data item (specified as an index in the list).
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            (e as Label).text = items[i];
        };

        // Provide the list view with an explict height for every row
        // so it can calculate how many items to actually display
        const int itemHeight = 80;

        //var listView = new ListView(items, itemHeight, makeItem, bindItem);

        empireList.itemsSource = items;
        empireList.makeItem = makeItem;
        empireList.bindItem = bindItem;
        empireList.selectionType = SelectionType.Multiple;

        empireList.onItemsChosen += objects => Debug.Log(objects);
        empireList.onSelectionChange += objects => Debug.Log(objects);

       // listView.style.flexGrow = 1.0f;

        empireList.fixedItemHeight = 200.0f;
        empireList.RefreshItems();
        //root.Add(listView);
    }
    */
    
    public void UpdateUI()
    {
        ClearResourceUnits();
        CreateResourceUnits();
        currentTurnLabel.text = "season " + (GameCenter.instance.currentTurn % GameCenter.instance.seasonsInAYear) + " year " + GameCenter.instance.currentTurn / 4;
        populationLimitLabel.text = "Pops " + GameCenter.instance.playerResources
                                        .GetMatchingResourceCategory(ResourceType.ResourceCategory.People)
                                        .Sum(e => e.amount) +
                                    "/" + GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(
                                        ResourceType.LinkType.MaxPopulation).amount;
        //playerMoneyLabel.text = GameCenter.instance.playerResources.GetStringDisplay();

    }
}
