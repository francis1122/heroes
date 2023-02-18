using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class UIResourceController : MonoBehaviour
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
        //onlyCategory.Sort((a,b) => a.buildingData.priority.CompareTo(b.buildingData.priority) );
        foreach (var resource in GameCenter.instance.playerResources.resources.FindAll(e =>
                     e.type.resourceCategory is ResourceType.ResourceCategory.Material))

        {
            ResourceData resourceData =
                GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(resource.type);
            ResourceData bufferResourceData =
                GameCenter.instance.playerBufferResources.GetOrCreateMatchingResourceType(resource.type);
            GameObject newResource = Instantiate(resourceTemplate, this.transform);
            // Vector3 newPos = newResource.transform.localPosition;
            // newPos.x = position * 150;
            //newResource.transform.localPosition = newPos;
            position++;

            newResource.GetComponent<UIResourceUnitController>().UpdateUIWithResource(resourceData, bufferResourceData);
            
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
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}