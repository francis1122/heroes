using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LandResourceController : MonoBehaviour
{
    
    // Start is called before the first frame update
    VisualElement root;


    public String container = "";
    private GroupBox resourceContainer;
    
    [SerializeField]
    VisualTreeAsset landResourceTemplate;


    public bool usePopulation = false;
    public ResourceType.ResourceCategory category = ResourceType.ResourceCategory.Unset;
    
    
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
        resourceContainer = root.Q<GroupBox>(container);
        //SetupEmpireList();
        
        UpdateUI();
    }

    public void CreateLandResourceUnits()
    {

        if (usePopulation)
        {
            foreach (var resource in GameCenter.instance.playerResources.populations)
            {
                TemplateContainer buildingBox = landResourceTemplate.Instantiate();

                //buildingBox.Q<Label>("resource-thumbnail")
                buildingBox.Q<Label>("land-owned").text =
                    resource.type.populationName + ": " + resource.amount.ToString();

                //buildingBox.Q<Label>("land-gain").text = "";
                if (resource.type.UITexture != null)
                {
                    buildingBox.Q<UIToolKitImage>("land-thumbnail").image = resource.type.UITexture;
                }

                resourceContainer.Add(buildingBox.Q<GroupBox>("land-unit"));


            }
        }
        else
        {

            foreach (var resource in GameCenter.instance.playerResources.GetMatchingResourceCategory(category))
            {
                TemplateContainer buildingBox = landResourceTemplate.Instantiate();

                //buildingBox.Q<Label>("resource-thumbnail")
                buildingBox.Q<Label>("land-owned").text =
                    resource.type.resourceName + ": " + resource.amount.ToString();

                //buildingBox.Q<Label>("land-gain").text = "";
                if (resource.type.UITexture != null)
                {
                    buildingBox.Q<UIToolKitImage>("land-thumbnail").image = resource.type.UITexture;
                }

                resourceContainer.Add(buildingBox.Q<GroupBox>("land-unit"));


            }
        }
    }

    public void ClearResourceUnits()
    {
        resourceContainer.Clear();
    }

    public void UpdateUI()
    {
        ClearResourceUnits();
        CreateLandResourceUnits();
        //playerMoneyLabel.text = GameCenter.instance.playerResources.GetStringDisplay();

    }
}
