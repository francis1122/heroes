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
        public Dictionary<PopulationType.LinkPopulationType, PopulationType> gamePopulationTypes = new();

        public ResourceOrganizer(ResourceType[] resourceTypes, PopulationType[] populationTypes)
        {

            foreach (var resourceType in resourceTypes)
            {
                gameResourceTypes.Add(resourceType.type, resourceType);
            }

            // foreach (var populationType in populationTypes)
            // {
            //     gamePopulationTypes.Add(populationType.type, populationType);
            // }
        }
        
        

        public ResourceType GetResourceType(ResourceType.LinkType linkType)
        {
            return gameResourceTypes[linkType];
        }
        
        public ResourceData CreateResourceData(int amount, ResourceType.LinkType linkType)
        {
            return new ResourceData(amount, gameResourceTypes[linkType]);
        }
        
        public PopulationType GetPopulationType(PopulationType.LinkPopulationType linkType)
        {
            return gamePopulationTypes[linkType];
        }
        
        public PopulationData CreatePopulationData(int amount, PopulationType.LinkPopulationType linkType)
        {
            return new PopulationData(amount,amount, gamePopulationTypes[linkType]);
        }
    }
}