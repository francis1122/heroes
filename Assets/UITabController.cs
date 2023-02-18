using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabController : MonoBehaviour
{
    
    
    [SerializeField]
    public List<GameObject> tabButtons;
    
    [SerializeField]
    public List<GameObject> tabPanels;
    
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < tabButtons.Count; i++)
        {
            
            GameObject button = tabButtons[i];
            button.GetComponent<Image>().color = Color.grey;
            
            GameObject panel = tabPanels[i];
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                for (int i = 0; i < tabButtons.Count; i++)
                {

                    GameObject buttonY = tabButtons[i];
                    buttonY.GetComponent<Image>().color = Color.grey;
                }
                button.GetComponent<Image>().color = Color.yellow;

                foreach (var tabPanel in tabPanels)
                {
                        tabPanel.SetActive(false);
                        
                }
                panel.SetActive(true);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
