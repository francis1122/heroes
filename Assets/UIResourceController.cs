using System.Collections.Generic;
using Data;
using UnityEngine;

public class UIResourceController : MonoBehaviour
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
            
            var newResource = activeResourceBoxes.Pop();
            newResource.SetActive(false);
            poolResourceBoxes.Push(newResource);
        }

        var position = 0;
        //onlyCategory.Sort((a,b) => a.buildingData.priority.CompareTo(b.buildingData.priority) );
        var resources = GameCenter.instance.playerResources.resources.FindAll(e =>
            e.type.resourceCategory is ResourceType.ResourceCategory.Material);
        resources.Reverse();
        foreach (var resource in resources)

        {
            ResourceData resourceData =
                GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(resource.type);
            ResourceData bufferResourceData =
                GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceType(resource.type);
            
            GameObject newResource = poolResourceBoxes.Pop();
            activeResourceBoxes.Push(newResource);
            newResource.SetActive(true);

            position++;

            newResource.GetComponent<UIResourceUnitController>().UpdateUIWithResource(resourceData, bufferResourceData);

        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}