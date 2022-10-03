using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Data
{
    [System.Serializable]
    public class ResourceBundle
    {
        [SerializeField] public List<ResourceData> resources = new();
        [SerializeField] public List<PopulationData> populations = new();
        public Boolean isPlayersResourceBundle = false;
        public Boolean isPlayersBufferResourceBundle = false;


        public ResourceBundle(ResourceBundle oldBundle, int scale)
        {
            foreach (var resourceData in oldBundle.resources)
            {
                ResourceData newData = new ResourceData(resourceData.amount * scale, resourceData.type);
                AddResourceData(newData);
            }
            
            foreach (var populationData in oldBundle.populations)
            {
                PopulationData newData = new PopulationData(populationData.amount * scale, populationData.type);
                AddPopulationData(newData);
            }
        }

        public void ClearResources()
        {
            resources = new();
            populations = new();
        }

        public ResourceData GetOrCreateMatchingResourceType(ResourceType resourceType)
        {
            foreach (ResourceData resource in resources)
            {
                if (resource.type == resourceType)
                {
                    return resource;
                }
            }

            ResourceData newResourceData = new ResourceData(0, resourceType);
            resources.Add(newResourceData);
            return newResourceData;
        }
        
        public PopulationData GetOrCreateMatchingPopulationType(PopulationType populationType)
        {
            foreach (PopulationData population in populations)
            {
                if (population.type == populationType)
                {
                    return population;
                }
            }

            PopulationData newResourceData = new PopulationData(0, populationType);
            populations.Add(newResourceData);
            return newResourceData;
        }
        
        
        
        public ResourceData GetOrCreateMatchingResourceLinkType(ResourceType.LinkType linkType)
        {
            return GetOrCreateMatchingResourceType(GameCenter.instance.resourceOrganizer.GetResourceType(linkType));
        }
        
        public PopulationData GetOrCreateMatchingPopulationLinkType(PopulationType.LinkPopulationType linkType)
        {
            return GetOrCreateMatchingPopulationType(GameCenter.instance.resourceOrganizer.GetPopulationType(linkType));
        }

        public List<ResourceData> GetMatchingResourceCategory(ResourceType.ResourceCategory category)
        {
            return resources.FindAll(e => e.type.resourceCategory == category);
        }

        public String GetStringDisplay()
        {
            String resourceString = "";
            foreach (ResourceData resource in resources.FindAll(e=> e.type.type != ResourceType.LinkType.Authority))
            {
                resourceString += resource.type.resourceName + " " + resource.amount + "   ";
            }

            return resourceString;
        }

        public bool CanSubtractResourceBundle(ResourceBundle subtractResourceBundle)
        {
            if (isPlayersBufferResourceBundle) return true;
            //test if this resource matches type and has proper amount to subtract 
            /*if (this.amount >= subtractionAmount.amount && this.type == subtractionAmount.type)
                return true;*/

            foreach (var subtractResourceData in subtractResourceBundle.resources)
            {

                if (!CanSubtractResourceData(subtractResourceData))
                {
                    Debug.Log("can't build");
                    return false;
                }
                
                /*if (!resourceData.CanSubtractResource(subtractResourceData))
                {
                    Debug.Log("dont have enough resources of type " + resourceData.type.resourceName);
                    return false;
                }*/
            }
            Debug.Log("can build");
            return true;
        }
        
        public bool CanSubtractResourceData(ResourceData subtractResourceData)
        {
            if (isPlayersBufferResourceBundle) return true;
            //test if this resource matches type and has proper amount to subtract 
            /*if (this.amount >= subtractionAmount.amount && this.type == subtractionAmount.type)
                return true;*/
            ResourceData resourceData = GetOrCreateMatchingResourceType(subtractResourceData.type);
            // test if this resource matches type and has proper amount to subtract 

            if (resourceData.type != subtractResourceData.type)
            {
                return false;
            }

            if (resourceData.amount >= subtractResourceData.amount || resourceData.type.amountCanBeNegative)
            {
                if (isPlayersResourceBundle && resourceData.type.checkForPlayerResourceMinLimit) 
                {
                    ResourceData playerMinResource = GameCenter.instance.playerMinResourceAmounts.GetOrCreateMatchingResourceType(resourceData.type);
                    if (resourceData.amount - subtractResourceData.amount < playerMinResource.amount)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
            return false;
        }

        
        
        
        public bool SubtractResourceBundle(ResourceBundle subtractResourceBundle)
        {

            foreach (var subtractResourceData in subtractResourceBundle.resources)
            {
                if (!SubtractResourceData(subtractResourceData))
                {
                    Debug.Log("Should call CanSubtractBundle to check purchase before");
           //         return false;
                }
            }

            foreach (var population in subtractResourceBundle.populations)
            {
                if (!SubtractPopulationData(population))
                {
                    Debug.Log("Should call CanSubtractBundle to check purchase before");
             //       return false;
                }
            }

            return true;
        }

        public bool SubtractResourceData(ResourceData subtractResourceData)
        {
            ResourceData resourceData = GetOrCreateMatchingResourceType(subtractResourceData.type);
            if (resourceData.amount >= subtractResourceData.amount || resourceData.type.amountCanBeNegative || isPlayersBufferResourceBundle)
            {
                if (isPlayersResourceBundle && resourceData.type.checkForPlayerResourceMinLimit)
                {
                    ResourceData playerMinResource = GameCenter.instance.playerMinResourceAmounts.GetOrCreateMatchingResourceType(resourceData.type);
                    if (resourceData.amount - subtractResourceData.amount >= playerMinResource.amount)
                    {
                        resourceData.amount -= subtractResourceData.amount;
                        return true;
                    }
                    Debug.Log("Should call CanSubtractResources to check purchase before player " + subtractResourceData.type.resourceName);
                    return false;
                }
                resourceData.amount -= subtractResourceData.amount;
                return true;
            }
            Debug.Log("Should call CanSubtractResources to check purchase before " + subtractResourceData.type.resourceName);
            return false;
        }
        
        public bool SubtractPopulationData(PopulationData subtractPopulationData)
        {
            PopulationData resourceData = GetOrCreateMatchingPopulationType(subtractPopulationData.type);
            if (resourceData.amount >= subtractPopulationData.amount || isPlayersBufferResourceBundle)
            {
                if (isPlayersResourceBundle && resourceData.type.checkForPlayerResourceMinLimit)
                {
                    PopulationData playerMinResource = GameCenter.instance.playerMinResourceAmounts.GetOrCreateMatchingPopulationType(resourceData.type);
                    if (resourceData.amount - subtractPopulationData.amount >= playerMinResource.amount)
                    {
                        resourceData.amount -= subtractPopulationData.amount;
                        return true;
                    }
                    Debug.Log("Should call CanSubtractResources to check purchase before player " + subtractPopulationData.type.populationName);
                    return false;
                }
                resourceData.amount -= subtractPopulationData.amount;
                return true;
            }
            Debug.Log("Should call CanSubtractResources to check purchase before " + subtractPopulationData.type.populationName);
            return false;
        }

        /*
        public bool CanAddResource(ResourceData addResourceData, bool canPartiallyAdd)
        {
            if (this.type != addResourceData.type) return false;
            //test if this resource matches type and has proper amount to subtract
            if (canPartiallyAdd)
            {
                return this.amount < this.maxAmount;
                
            }
            else
            {
                int spaceAvailable = this.maxAmount - this.amount;
                return addResourceData.amount <= spaceAvailable;
                
            }
 
        }
        */
        
        public void AddResourceBundle(ResourceBundle addResourceBundle)
        {
            foreach (var addResourceData in addResourceBundle.resources)
            {
                AddResourceData(addResourceData);

            }

            foreach (var addPopulationData in addResourceBundle.populations)
            {
                AddPopulationData(addPopulationData);
            }
        }

        public void AddResourceData(ResourceData addResourceData)
        {
            ResourceData resourceData = GetOrCreateMatchingResourceType(addResourceData.type);
            resourceData.amount += addResourceData.amount;

            // check and reduce to max limit if resource 
            if (isPlayersResourceBundle)
            {
                
                if (resourceData.type.checkForPlayerResourceMaxLimit)
                {
                    
                    ResourceData playerMaxResource = GameCenter.instance.playerMaxResourceAmounts.GetOrCreateMatchingResourceType(resourceData.type);
                    if (resourceData.amount > playerMaxResource.amount)
                    {
                        
                        resourceData.amount = playerMaxResource.amount;
                    }
                }
            }
        }
        public void AddPopulationData(PopulationData addPopulationData)
        {
            PopulationData resourceData = GetOrCreateMatchingPopulationType(addPopulationData.type);
            resourceData.amount += addPopulationData.amount;

            // check and reduce to max limit if resource 
            if (isPlayersResourceBundle)
            {
                
                if (resourceData.type.checkForPlayerResourceMaxLimit)
                {
                    PopulationData playerMaxResource = GameCenter.instance.playerMaxResourceAmounts.GetOrCreateMatchingPopulationType(resourceData.type);
                    if (resourceData.amount > playerMaxResource.amount)
                    {
                        resourceData.amount = playerMaxResource.amount;
                    }
                }
            }
        }
        
        
        //
        //
        //

        public void EndOfTurnTriggers()
        {
            foreach (var resourceData in GameCenter.instance.playerResources.resources)
            {
                foreach (var trigger in resourceData.type.playerEndOfTurnTriggers)
                {
                    trigger.Trigger();
                }
            }
            
            foreach (var resourceData in GameCenter.instance.playerResources.populations)
            {
                foreach (var trigger in resourceData.type.playerEndOfTurnTriggers)
                {
                    trigger.Trigger();
                }
            }
        }
    }
}
