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
            GameObject panel = tabPanels[i];
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
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
