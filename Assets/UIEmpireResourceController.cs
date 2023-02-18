using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class UIEmpireResourceController : MonoBehaviour
{
    [SerializeField] GameObject resourceTemplate;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
        EventManager.StartListening(EventManager.EVENT_END_TURN, UpdateUI);
        EventManager.StartListening(EventManager.RESOURCES_CHANGED, UpdateUI);
    }


    public void UpdateUI()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var position = 0;
        // foreach (var resource in GameCenter.instance.playerResources.resources.FindAll(e =>
        //              e.type.resourceCategory is ResourceType.ResourceCategory.Empire))
        //
        // {

        // could treat each one 


        // MilitaryPower
        CreateAuthUI();
        CreateMilitaryUI();
        CreateStabilityUI();
        CreatePopulationUI();


        // Vector3 newPos = newResource.transform.localPosition;
        // newPos.x = position * 150;
        //newResource.transform.localPosition = newPos;
        //position++;



        // TemplateContainer buildingBox = resourceTemplate.Instantiate();
        //
        // //buildingBox.Q<Label>("resource-thumbnail")
        // buildingBox.Q<Label>("resource-owned").text = resource.type.resourceName + ": " + resource.amount.ToString();
        // ResourceData bufferResourceData =
        //     GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceType(resource.type);
        //
        // buildingBox.Q<Label>("resource-gain").text = bufferResourceData.amount.ToString();
        // if (resource.type.UITexture != null)
        // {
        //     buildingBox.Q<UIToolKitImage>("resource-thumbnail").image = resource.type.UITexture;
        // }
        // resourceContainer.Add(buildingBox.Q<GroupBox>("resource-unit"));
        // }
    }

    // Update is called once per frame


    void CreateAuthUI()
    {
        ResourceData maxResource = GameCenter.instance.playerMaxResourceAmounts.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority);
        ResourceData resourceData =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority);
            
        ResourceData bufferResourceData =
            GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority);
        GameObject newResource = Instantiate(resourceTemplate, this.transform);
        newResource.GetComponent<UIResourceUnitController>().UpdateUIWithResourceLimit(resourceData, maxResource, bufferResourceData);

    }

void CreateMilitaryUI()
    {
        ResourceData maxResource = GameCenter.instance.playerMaxResourceAmounts.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MilitaryPower);
        ResourceData resourceData =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MilitaryPower);
            
        ResourceData bufferResourceData =
            GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MilitaryPower);
        GameObject newResource = Instantiate(resourceTemplate, this.transform);
        newResource.GetComponent<UIResourceUnitController>().UpdateUIWithResourceLimit(resourceData, maxResource, bufferResourceData);

    }

    void CreateStabilityUI()
    {
        ResourceData resourceData =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Stability);
            
        ResourceData bufferResourceData =
            GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Stability);
        GameObject newResource = Instantiate(resourceTemplate, this.transform);
        newResource.GetComponent<UIResourceUnitController>().UpdateUIWithResource(resourceData, bufferResourceData);
 
    }

    void CreatePopulationUI()
    {
        ResourceData resourceData =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MaxPopulation);
            
        ResourceData bufferResourceData =
            GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MaxPopulation);
        GameObject newResource = Instantiate(resourceTemplate, this.transform);
        newResource.GetComponent<UIResourceUnitController>().UpdateUIWithResource(resourceData, bufferResourceData);

    }
    
    void Update()
    {
    }
}
