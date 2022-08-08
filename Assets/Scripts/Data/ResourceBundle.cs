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

        public String GetStringDisplay()
        {
            String resourceString = "";
            foreach (ResourceData resource in resources)
            {
                resourceString += resource.type.resourceName + " " + resource.amount + "   ";
            }

            return resourceString;
        }

        public bool CanSubtractResource(ResourceBundle subtractResourceBundle)
        {
            //test if this resource matches type and has proper amount to subtract 
            /*if (this.amount >= subtractionAmount.amount && this.type == subtractionAmount.type)
                return true;*/

            foreach (var subtractResourceData in subtractResourceBundle.resources)
            {
                ResourceData resourceData = GetOrCreateMatchingResourceType(subtractResourceData.type);
                if (!resourceData.CanSubtractResource(subtractResourceData))
                {
                    Debug.Log("dont have enough resources of type " + resourceData.type.resourceName);
                    return false;
                }
            }

            return true;
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