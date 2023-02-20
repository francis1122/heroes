using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class UIEmpireResourceController : MonoBehaviour
{
    [SerializeField] GameObject resourceTemplate;

    public Stack<GameObject> activeResourceBoxes = new Stack<GameObject>();
    public Stack<GameObject> poolResourceBoxes = new Stack<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < 30; i++)
        {
            GameObject newResourceCard = Instantiate(resourceTemplate, this.transform);
            newResourceCard.SetActive(false);
            poolResourceBoxes.Push(newResourceCard);
        }
        
        UpdateUI();
        EventManager.StartListening(EventManager.EVENT_END_TURN, UpdateUI);
        EventManager.StartListening(EventManager.RESOURCES_CHANGED, UpdateUI);
    }


    public void UpdateUI()
    {
        while (activeResourceBoxes.Count > 0)
        {
            
            var activeBuilding = activeResourceBoxes.Pop();
            activeBuilding.SetActive(false);
            activeBuilding.transform.parent = null;
            //test.transform.parent = this.transform;
            poolResourceBoxes.Push(activeBuilding);
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
        CreateHappinessUI();

    }

    // Update is called once per frame


    void CreateAuthUI()
    {
        ResourceData maxResource = GameCenter.instance.playerMaxResourceAmounts.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority);
        ResourceData resourceData =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority);
            
        ResourceData bufferResourceData =
            GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority);
        
        GameObject newResource = poolResourceBoxes.Pop();
        activeResourceBoxes.Push(newResource);
        newResource.SetActive(true);
        newResource.transform.parent = transform;
        
        newResource.GetComponent<UIResourceUnitController>().UpdateUIWithResourceLimit(resourceData, maxResource, bufferResourceData);

    }

void CreateMilitaryUI()
    {
        ResourceData maxResource = GameCenter.instance.playerMaxResourceAmounts.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MilitaryPower);
        ResourceData resourceData =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MilitaryPower);
            
        ResourceData bufferResourceData =
            GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MilitaryPower);
        GameObject newResource = poolResourceBoxes.Pop();
        activeResourceBoxes.Push(newResource);
        newResource.SetActive(true);
        newResource.transform.parent = transform;
        newResource.GetComponent<UIResourceUnitController>().UpdateUIWithResourceLimit(resourceData, maxResource, bufferResourceData);

    }

    void CreateStabilityUI()
    {
        ResourceData resourceData =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Stability);
            
        ResourceData bufferResourceData =
            GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Stability);
        GameObject newResource = poolResourceBoxes.Pop();
        activeResourceBoxes.Push(newResource);
        newResource.SetActive(true);
        newResource.transform.parent = transform;
        newResource.GetComponent<UIResourceUnitController>().UpdateUIWithResource(resourceData, bufferResourceData);
 
    }
    
    void CreateHappinessUI()
    {
        ResourceData resourceData =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Happiness);
            
        ResourceData bufferResourceData =
            GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Happiness);
        GameObject newResource = poolResourceBoxes.Pop();
        activeResourceBoxes.Push(newResource);
        newResource.SetActive(true);
        newResource.transform.parent = transform;
        newResource.GetComponent<UIResourceUnitController>().UpdateUIWithResource(resourceData, bufferResourceData);
 
    }

    void CreatePopulationUI()
    {
        ResourceData resourceData =
            GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MaxPopulation);
            
        ResourceData bufferResourceData =
            GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.MaxPopulation);
        GameObject newResource = poolResourceBoxes.Pop();
        activeResourceBoxes.Push(newResource);
        newResource.SetActive(true);
        newResource.transform.parent = transform;
        newResource.GetComponent<UIResourceUnitController>().UpdateUIWithResource(resourceData, bufferResourceData);

    }
    
    void Update()
    {
    }
}
