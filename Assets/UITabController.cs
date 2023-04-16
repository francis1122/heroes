using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabController : MonoBehaviour
{


    private int currentPanelIndex = 0;
    
    [SerializeField]
    public List<GameObject> tabButtons;
    
    [SerializeField]
    public List<GameObject> tabPanels;

    [SerializeField] public GameObject scrollView;
    
    // Start is called before the first frame update
    void Start()
    {
        
        // Get event for whenever a new 'building'e gets added to player
        EventManager.StartListening(EventManager.BUILDING_CHANGED, BadgeUpdate);
        //hasBeenSeen
        
        for (int i = 0; i < tabButtons.Count; i++)
        {
            
            GameObject button = tabButtons[i];
            GameObject panel = tabPanels[i];
            int index = i;
            button.GetComponent<Image>().color = Color.grey;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                NavigateToPanelIndex(index);
            });
        }
    }

    public void BadgeUpdate()
    {
        // get count of hasBeenSeen
    }
    
    void NavigateToPanelIndex(int index)
    {
        
        GameObject button = tabButtons[index];
        GameObject panel = tabPanels[index];
        for (int j = 0; j < tabButtons.Count; j++)
        {
            GameObject buttonY = tabButtons[j];
            GameObject tabPanelY = tabPanels[j];
            tabPanelY.SetActive(false);
            buttonY.GetComponent<Image>().color = Color.grey;
        }

        button.GetComponent<Image>().color = Color.yellow;
                
        scrollView.GetComponent<ScrollRect>().content = panel.GetComponent<RectTransform>();
        panel.SetActive(true);
        
        // panel that is being moved away from, set all buildings to have been seend
        
        currentPanelIndex = index;
    }

    public void NavigateRight()
    {
        if (currentPanelIndex < tabButtons.Count - 1)
        {
            NavigateToPanelIndex(currentPanelIndex + 1);
        }   
    }
    
    public void NavigateLeft()
    {
        if (currentPanelIndex > 0)
        {
            NavigateToPanelIndex(currentPanelIndex - 1);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
