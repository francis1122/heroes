using System;
using Data;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class ScaleResources
    {
        public ResourceType.LinkType scaleWithLinkType = ResourceType.LinkType.Unset;
        public PopulationType.LinkPopulationType scaleWithPopulationLinkType = PopulationType.LinkPopulationType.Unset;

        [SerializeField, SerializeReference]
        public BuildingData scaleWithBuildingOwned;

        public bool scaleByOwnRecentUsage = false;


        public float GetScaler()
        {
            if (scaleWithLinkType != ResourceType.LinkType.Unset)
            {
                int amount = GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(scaleWithLinkType)
                    .amount;
                return Math.Max(1, amount);
            }else if (scaleWithPopulationLinkType != PopulationType.LinkPopulationType.Unset){
                int amount = GameCenter.instance.playerResources.GetOrCreateMatchingPopulationLinkType(scaleWithPopulationLinkType)
                    .amount;
                return Math.Max(1, amount);
            }else if (scaleWithBuildingOwned != null)
            {
                
            }else if (scaleByOwnRecentUsage)
            {
                
            }
            
            return 1.0f;
        }

    }
}