using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public class ResourceStatusEffects
    {

        [System.Serializable]
        public class ResourceStatus
        {
            [SerializeField]
            public ResourceType.LinkType resourceType =  ResourceType.LinkType.Unset;
            
            [SerializeField]
            public int valueDelta = 0;
        }

        [System.Serializable]
        public class PopulationStatus
        {
            [SerializeField]
            public PopulationType.LinkPopulationType populationType = PopulationType.LinkPopulationType.Unset;
            
            [SerializeField]
            public int valueDelta = 0;
        }
        
        [SerializeField]
        public List<ResourceStatus> resourceStatusList = new();

        [SerializeField]
        public List<PopulationStatus> populationStatusList = new ();
        
        // identifier
        [SerializeField]
        public StatusIdentifier statusIdentifier = new StatusIdentifier();

        public int GetResourceStatus(ResourceType.LinkType linkType)
        {
            foreach (var resourceStatus in resourceStatusList)
            {
                if (resourceStatus.resourceType == linkType)
                {
                    return resourceStatus.valueDelta;
                }
            }

            return 0;
        }
        
        public int GetPopulationStatus(PopulationType.LinkPopulationType linkType)
        {
            foreach (var resourceStatus in populationStatusList)
            {
                if (resourceStatus.populationType == linkType)
                {
                    return resourceStatus.valueDelta;
                }
            }

            return 0;
        }
    }
}
