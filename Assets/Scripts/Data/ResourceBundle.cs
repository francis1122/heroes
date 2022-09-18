using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class ResourceBundle
    {
        [SerializeField] public List<ResourceData> resources = new();
        public Boolean isPlayersResourceBundle = false;
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
        
        public ResourceData GetOrCreateMatchingResourceLinkType(ResourceType.LinkType linkType)
        {
            return GetOrCreateMatchingResourceType(GameCenter.instance.resourceOrganizer.GetResourceType(linkType));
        }

        public List<ResourceData> GetMatchingResourceCategory(ResourceType.ResourceCategory category)
        {
            return resources.FindAll(e => e.type.resourceCategory == category);
        }

        public String GetStringDisplay()
        {
            String resourceString = "";
            foreach (ResourceData resource in resources)
            {
                resourceString += resource.type.resourceName + " " + resource.amount + "   ";
            }

            return resourceString;
        }

        public bool CanSubtractResourceBundle(ResourceBundle subtractResourceBundle)
        {
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
                    return false;
                }
            }

            return true;
        }

        public bool SubtractResourceData(ResourceData subtractResourceData)
        {
            ResourceData resourceData = GetOrCreateMatchingResourceType(subtractResourceData.type);
            if (resourceData.amount >= subtractResourceData.amount || resourceData.type.amountCanBeNegative)
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

        // returns the amount that was added
        public void AddResourceBundle(ResourceBundle addResourceBundle)
        {
            foreach (var addResourceData in addResourceBundle.resources)
            {
                AddResourceData(addResourceData);

            }

            /*this.amount += addResourceData.amount;
            return addResourceData;*/
            /*int spaceAvailable = this.maxAmount - this.amount;
            int amountToAdd = Math.Min(spaceAvailable, addResourceData.amount);
            this.amount += amountToAdd;
            ResourceData subtracted = new ResourceData(addResourceData);
            subtracted.amount = amountToAdd; 
            return subtracted;*/
        }

        public void AddResourceData(ResourceData addResourceData)
        {
            ResourceData resourceData = GetOrCreateMatchingResourceType(addResourceData.type);
            resourceData.amount += addResourceData.amount;
            Debug.Log("cull max res 0");
            // check and reduce to max limit if resource 
            if (isPlayersResourceBundle)
            {
                Debug.Log("cull max res 1");
                if (resourceData.type.checkForPlayerResourceMaxLimit)
                {
                    Debug.Log("cull max res 2");
                    ResourceData playerMaxResource = GameCenter.instance.playerMaxResourceAmounts.GetOrCreateMatchingResourceType(resourceData.type);
                    if (resourceData.amount > playerMaxResource.amount)
                    {
                        Debug.Log("cull max res 3");
                        resourceData.amount = playerMaxResource.amount;
                    }
                }
            }
        }



        /*[MenuItem("Tools/ResourceBundle")]
        public static void CreateMyAsset()
        {
            ResourceBundle asset = ScriptableObject.CreateInstance<ResourceBundle>();
    
            AssetDatabase.CreateAsset(asset, "Assets/Data/ResourceBundle/NewResourceBundle.asset");
            AssetDatabase.SaveAssets();
    
            EditorUtility.FocusProjectWindow();
    
            Selection.activeObject = asset;
        }*/
    }
}