using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Data
{
[System.Serializable]
    public class ResourceOrganizer
    {
        public Dictionary<ResourceType.LinkType, ResourceType> gameResourceTypes = new();

        public ResourceOrganizer(ResourceType[] resourceTypes)
        {

            foreach (var resourceType in resourceTypes)
            {
                gameResourceTypes.Add(resourceType.type, resourceType);
            }
        }
        
        

        public ResourceType GetResourceType(ResourceType.LinkType linkType)
        {
            return gameResourceTypes[linkType];
        }
        
        public ResourceData CreateResourceData(int amount, ResourceType.LinkType linkType)
        {
            return new ResourceData(amount, gameResourceTypes[linkType]);
        }
    }
}