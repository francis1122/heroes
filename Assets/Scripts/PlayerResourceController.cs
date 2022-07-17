using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerResourceController : MonoBehaviour
{
    // Start is called before the first frame update
    VisualElement root;

    private Label playerMoneyLabel;
    private Label currentTurnLabel;
    private Button endTurnButton;

    private ListView empireList;
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
        playerMoneyLabel = root.Q<Label>("player-money-label");
        currentTurnLabel = root.Q<Label>("turn-number-label");
        endTurnButton = root.Q<Button>("end-turn-button");
        //SetupEmpireList();
        
        
        endTurnButton.RegisterCallback<ClickEvent>((evt =>
        {
            GameCenter.instance.EndTurn();
        }));
        UpdateUI();
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
        currentTurnLabel.text = "turn " + GameCenter.instance.currentTurn;
        playerMoneyLabel.text = GameCenter.instance.playerResources.GetStringDisplay();

    }
}
