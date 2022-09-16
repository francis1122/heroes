using UnityEngine;
using UnityEngine.UIElements;

    public class TabController : MonoBehaviour
    {
        // Start is called before the first frame update
        VisualElement root;

            
        void Start()
        {
            UIDocument menu = GetComponent<UIDocument>();
            root = menu.rootVisualElement;

            var buildingPanel = root.Q<GroupBox>("building-panel");
            var actionsPanel = root.Q<GroupBox>("action-panel");
            var populationPanel = root.Q<GroupBox>("population-panel");
            
            var buildingButton = root.Q<Button>("tab-buildings");
            var actionButton = root.Q<Button>("tab-actions");
            var populationButton = root.Q<Button>("tab-population");
            
            buildingButton.RegisterCallback<ClickEvent>((evt =>
            {
                buildingPanel.style.display = DisplayStyle.Flex;
                actionsPanel.style.display = DisplayStyle.None;
                populationPanel.style.display = DisplayStyle.None;
            }));
            
            actionButton.RegisterCallback<ClickEvent>((evt =>
            {
                buildingPanel.style.display = DisplayStyle.None;
                actionsPanel.style.display = DisplayStyle.Flex;
                populationPanel.style.display = DisplayStyle.None;
            }));
            
            populationButton.RegisterCallback<ClickEvent>((evt =>
            {
                buildingPanel.style.display = DisplayStyle.None;
                actionsPanel.style.display = DisplayStyle.None;
                populationPanel.style.display = DisplayStyle.Flex;
            }));

            
        
            /*
            EventManager.StartListening(EventManager.EVENT_END_TURN, UpdateUI );
            EventManager.StartListening(EventManager.RESOURCES_CHANGED, UpdateUI );
            EventManager.StartListening(EventManager.BUILDING_CHANGED, UpdateUI );
            */
        
        }
    }