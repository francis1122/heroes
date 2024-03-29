﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Data
{
    [System.Serializable]
    public class ResourceBundle
    {
        [SerializeField] public List<ResourceData> resources = new();

        //public int population = 0;
        //[SerializeField] public List<PopulationData> populations = new();
        
        public Boolean isPlayersResourceBundle = false;
        public Boolean isPlayersBufferResourceBundle = false;


        public ResourceBundle()
        {
            
        }
        
        public ResourceBundle(ResourceBundle oldBundle, int scale)
        {
            foreach (var resourceData in oldBundle.resources)
            {
                ResourceData newData = new ResourceData(resourceData.amount * scale, resourceData.type);
                AddResourceData(newData);
            }
            
            // foreach (var populationData in oldBundle.populations)
            // {
            //     PopulationData newData = new PopulationData(populationData.amount * scale, populationData.activeAmount * scale, populationData.type);
            //     AddPopulationData(newData);
            // }
        }
        
        public ResourceBundle(ResourceBundle oldBundle, ResourceStatusEffects resourceStatusEffects)
        {
            foreach (var resourceData in oldBundle.resources)
            {
                int change = resourceStatusEffects.GetResourceStatus(resourceData.type.type);
                ResourceData newData = new ResourceData(resourceData.amount + change, resourceData.type);
                AddResourceData(newData);
            }
            
            // foreach (var populationData in oldBundle.populations)
            // {
            //     int change = resourceStatusEffects.GetPopulationStatus(populationData.type.type);
            //     PopulationData newData = new PopulationData(populationData.amount + change, populationData.activeAmount + change, populationData.type);
            //     AddPopulationData(newData);
            // }
        }

        public void ClearResources()
        {
            resources = new();
            //populations = new();
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
        
        // public PopulationData GetOrCreateMatchingPopulationType(PopulationType populationType)
        // {
        //     foreach (PopulationData population in populations)
        //     {
        //         if (population.type == populationType)
        //         {
        //             return population;
        //         }
        //     }
        //
        //     PopulationData newResourceData = new PopulationData(0,0,  populationType);
        //     populations.Add(newResourceData);
        //     return newResourceData;
        // }
        
        
        
        public ResourceData GetOrCreateMatchingResourceLinkType(ResourceType.LinkType linkType)
        {
            return GetOrCreateMatchingResourceType(GameCenter.instance.resourceOrganizer.GetResourceType(linkType));
        }
        
        // public PopulationData GetOrCreateMatchingPopulationLinkType(PopulationType.LinkPopulationType linkType)
        // {
        //     return GetOrCreateMatchingPopulationType(GameCenter.instance.resourceOrganizer.GetPopulationType(linkType));
        // }

        public List<ResourceData> GetMatchingResourceCategory(ResourceType.ResourceCategory category)
        {
            return resources.FindAll(e => e.type.resourceCategory == category);
        }

        // public String GetPopulationStringDisplay()
        // {
        //     String resourceString = "";
        //     foreach (PopulationData populationData in populations)
        //     {
        //         resourceString += populationData.type.populationName + " (" + populationData.activeAmount + ")" + populationData.amount + "";
        //     }
        //     
        //     return resourceString;
        // }
        
        // public String GetPopulationRecruitAvailableStringDisplay()
        // {
        //     //ResourceBundle playerMaxResourceAmounts = GameCenter.instance.playerMaxResourceAmounts;
        //     ResourceBundle playerResourceBundle = GameCenter.instance.playerResources;
        //     
        //     String resourceString = "";
        //     foreach (PopulationData populationData in populations)
        //     {
        //         var playerPop = playerResourceBundle.GetOrCreateMatchingPopulationType(populationData.type);
        //       //  var maxPlayerPop = playerMaxResourceAmounts.GetOrCreateMatchingPopulationType(populationData.type);
        //         if(populationData.amount > 0)
        //         resourceString +=  playerPop.amount;
        //     }
        //     
        //     return resourceString;
        // }

        public String GetResourceStringDisplay()
        {
            String resourceString = "";
            foreach (ResourceData resource in resources.FindAll(e=> e.type.type != ResourceType.LinkType.Authority))
            {
                resourceString += resource.type.resourceName + " " + resource.amount + "  ";
            }

            return resourceString;
        }
        
        public String GetStringDisplay()
        {

            String resourceString = GetResourceStringDisplay();

           // String popCost = GetPopulationStringDisplay();
            // if (popCost.NullIfEmpty() != null)
            // {
            //     resourceString += " ";
            //     resourceString += popCost;
            // }

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
                    Debug.Log("can't build: not enough resources");
                    return false;
                }
                
                /*if (!resourceData.CanSubtractResource(subtractResourceData))
                {
                    Debug.Log("dont have enough resources of type " + resourceData.type.resourceName);
                    return false;
                }*/
            }

            // foreach (var populationData in subtractResourceBundle.populations)
            // {
            //     if (!CanSubtractPopulationData(populationData))
            //     {
            //         Debug.Log("can't build: not enough population");
            //         return false;
            //     }
            // }

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

        // public bool CanSubtractPopulationData(PopulationData subtractPopulationData)
        // {
        //     if (isPlayersBufferResourceBundle) return true;
        //     //test if this resource matches type and has proper amount to subtract 
        //     /*if (this.amount >= subtractionAmount.amount && this.type == subtractionAmount.type)
        //         return true;*/
        //     PopulationData populationData = GetOrCreateMatchingPopulationType(subtractPopulationData.type);
        //     // test if this resource matches type and has proper amount to subtract 
        //
        //     if (populationData.type != subtractPopulationData.type)
        //     {
        //         return false;
        //     }
        //
        //  //   if (populationData.annualRecruitsAvailable >= subtractPopulationData.annualRecruitsAvailable)
        //   //  {
        //         if ((populationData.amount >= subtractPopulationData.amount
        //              && populationData.activeAmount >= subtractPopulationData.activeAmount)
        //             || populationData.type.amountCanBeNegative)
        //         {
        //             if (isPlayersResourceBundle && populationData.type.checkForPlayerResourceMinLimit)
        //             {
        //                 PopulationData playerMinResource =
        //                     GameCenter.instance.playerMinResourceAmounts.GetOrCreateMatchingPopulationType(
        //                         populationData.type);
        //                 if (populationData.amount - subtractPopulationData.amount < playerMinResource.amount)
        //                 {
        //                     return true;
        //                 }
        //
        //                 return false;
        //             }
        //
        //             return true;
        //         }
        //  //   }
        //
        //     return false;
        // }
        
        
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

            // foreach (var population in subtractResourceBundle.populations)
            // {
            //     if (!SubtractPopulationData(population))
            //     {
            //         Debug.Log("Should call CanSubtractBundle to check purchase before");
            //  //       return false;
            //     }
            // }

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
        
        // public bool SubtractPopulationData(PopulationData subtractPopulationData)
        // {
        //     PopulationData populationData = GetOrCreateMatchingPopulationType(subtractPopulationData.type);
        //     
        //     if ((populationData.amount >= subtractPopulationData.amount 
        //          && populationData.activeAmount >= subtractPopulationData.activeAmount )
        //        //  && populationData.annualRecruitsAvailable >= subtractPopulationData.annualRecruitsAvailable
        //        //  && populationData.annualRecruitLimit >= subtractPopulationData.annualRecruitLimit) 
        //         || isPlayersBufferResourceBundle)
        //     {
        //         if (isPlayersResourceBundle && populationData.type.checkForPlayerResourceMinLimit)
        //         {
        //             PopulationData playerMinResource = GameCenter.instance.playerMinResourceAmounts.GetOrCreateMatchingPopulationType(populationData.type);
        //             if (populationData.amount - subtractPopulationData.amount >= playerMinResource.amount)
        //             {
        //                 populationData.amount -= subtractPopulationData.amount;
        //                 populationData.activeAmount -= subtractPopulationData.activeAmount;
        //                 //populationData.annualRecruitsAvailable -= subtractPopulationData.annualRecruitsAvailable;
        //                 return true;
        //             }
        //             Debug.Log("Should call CanSubtractResources to check purchase before player " + subtractPopulationData.type.populationName);
        //             return false;
        //         }
        //         populationData.amount -= subtractPopulationData.amount;
        //         populationData.activeAmount -= subtractPopulationData.activeAmount;
        //         //populationData.annualRecruitsAvailable -= subtractPopulationData.annualRecruitsAvailable;
        //        // populationData.annualRecruitLimit -= subtractPopulationData.annualRecruitLimit;
        //         return true;
        //     }
        //     Debug.Log("Should call CanSubtractResources to check purchase before " + subtractPopulationData.type.populationName);
        //     return false;
        // }

        
        public bool CanAddResourceBundle(ResourceBundle addResourceBundle, bool canPartiallyAdd = true)
        {
            // Check max limits
            foreach (var addResourceData in addResourceBundle.resources)
            {
                if (!CanAddResourceData(addResourceData, canPartiallyAdd))
                {
                    return false;
                }

            }

            // foreach (var addPopulationData in addResourceBundle.populations)
            // {
            //     if (!CanAddPopulationData(addPopulationData, canPartiallyAdd))
            //     {
            //         return false;
            //     }
            // }

            return true;
        }
        
        
        public void AddResourceBundle(ResourceBundle addResourceBundle, bool canPartiallyAdd = true)
        {
            foreach (var addResourceData in addResourceBundle.resources)
            {
                AddResourceData(addResourceData);

            }
            //
            // foreach (var addPopulationData in addResourceBundle.populations)
            // {
            //     AddPopulationData(addPopulationData);
            // }
        }

        public bool CanAddResourceData(ResourceData addResourceData, bool canPartiallyAdd = true)
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

            return true;
        }
        // public bool CanAddPopulationData(PopulationData addPopulationData, bool canPartiallyAdd = true)
        // {
        //     PopulationData populationData = GetOrCreateMatchingPopulationType(addPopulationData.type);
        //     //populationData.AddResource(addPopulationData);
        //     int newPopAmount = populationData.amount + addPopulationData.amount;
        //     // check and reduce to max limit if resource 
        //     if (isPlayersResourceBundle)
        //     {
        //         
        //         if (populationData.type.checkForPlayerResourceMaxLimit && !canPartiallyAdd)
        //         {
        //             PopulationData playerMaxResource = GameCenter.instance.playerMaxResourceAmounts.GetOrCreateMatchingPopulationType(populationData.type);
        //             if (newPopAmount > playerMaxResource.amount)
        //             {
        //                 return false;
        //             }
        //         }
        //     }
        //
        //     return true;
        // }
        public void AddResourceData(ResourceData addResourceData, bool canPartiallyAdd = true)
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

                if (!resourceData.type.amountCanBeNegative && resourceData.amount < 0)
                {
                    resourceData.amount = 0;
                }
            }
        }
        // public void AddPopulationData(PopulationData addPopulationData, bool canPartiallyAdd = true)
        // {
        //     PopulationData populationData = GetOrCreateMatchingPopulationType(addPopulationData.type);
        //     populationData.AddResource(addPopulationData);
        //
        //     // check and reduce to max limit if resource 
        //     if (isPlayersResourceBundle)
        //     {
        //         
        //         if (populationData.type.checkForPlayerResourceMaxLimit)
        //         {
        //             PopulationData playerMaxResource = GameCenter.instance.playerMaxResourceAmounts.GetOrCreateMatchingPopulationType(populationData.type);
        //             if (populationData.amount > playerMaxResource.amount)
        //             {
        //                 populationData.amount = playerMaxResource.amount;
        //             }
        //         }
        //     }
        // }
        //
        
        //
        //
        //

        public void EndOfTurnTriggers()
        {
            foreach (var resourceData in GameCenter.instance.playerResources.resources)
            {
                foreach (var trigger in resourceData.type.playerEndOfTurnTriggers)
                {
                    trigger.Trigger(new StatusIdentifier());
                }
            }
            
            // foreach (var resourceData in GameCenter.instance.playerResources.populations)
            // {
            //     foreach (var trigger in resourceData.type.playerEndOfTurnTriggers)
            //     {
            //         trigger.Trigger((new StatusIdentifier()));
            //     }
            // }
        }
    }
}
