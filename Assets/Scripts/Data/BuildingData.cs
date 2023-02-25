using System;
using System.Collections.Generic;
using Triggers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Data
{
    public class BuildingData : ScriptableObject
    {
        public enum BuildingCategory
        {
            Building,
            Action,
            Population,
            Event
        }
        
        public string buildingName = "";
        [Header("do not edit")]
        public string uniqueName = "";
        public string buildingDetails = "";
        public string buildingThumbnail;

        public ResourceBundle costRequirements;
        public ScaleResources costScaleResources = null;

        [SerializeField] public int prestigeGainedOnPurchase = 2;
        [System.Serializable]
        public struct BuildingBundle
        {
            [SerializeField]
            public int amount;
            [SerializeField]
            public BuildingData buildingData;
        }


        public BuildingCategory category = BuildingCategory.Building;
        public BuildingBundle[] buildingRequirement = Array.Empty<BuildingBundle>();
        [Header("reduce owned amount")]
        public bool sellRequiredBuildings = false;
        [Header("removes them from the game")]
        public bool destroyRequiredBuildings = false;
        [Header("allow only one of the building")]
        public bool destroyOnPurchase = false;
        public bool repeatablePurchase = false;
        public bool addToOwnedBuildings = true;

        public List<BuildingData> buildingAdditionsOnPurchase;
        public int priority = 1;

        

        
        [Header("population UI - pop ")] public PopulationType populationGain;
        
        [SerializeField]
        public List<GameTriggers> onPurchaseTrigger = new();
        [SerializeField]
        public List<GameTriggers> onTurnEndTrigger = new();
        [SerializeField]
        public List<GameTriggers> onYearEndTrigger = new();

        [Header("Event specific ")] 
        
        public int eventLifeSpan = 4;
        [SerializeField]
        public List<GameTriggers> onExpiredEvent = new List<GameTriggers>();


        public String GetBuildingCostAndRequirementString()
        {
            String costRequirements = this.ScaledResourceBundle().GetStringDisplay();
            String buildingRequirementString = GetBuildingRequirementString();
            if (buildingRequirementString.NullIfEmpty() != null)
            {
                costRequirements += "\n";
                costRequirements += buildingRequirementString;
            }
            return costRequirements;
        }
        
        public String GetBuildingRequirementString()
        {
            String buildingRequirementString = "";
            foreach (var building in buildingRequirement)
            {
                if (building.buildingData != null)
                {
                    buildingRequirementString += building.buildingData.buildingName + " ";
                }
            }

            return buildingRequirementString;
        }

        // public String GetBuildingPopulationGainMax()
        // {
        //     if (category == BuildingCategory.Population && populationGain != null)
        //     {
        //         ResourceBundle playerResourceAmounts = GameCenter.instance.playerResources;
        //         ResourceBundle playerMaxResourceAmounts = GameCenter.instance.playerMaxResourceAmounts;
        //         var maxPlayerPop = playerResourceAmounts.GetOrCreateMatchingPopulationType(populationGain).amount + " - " + playerMaxResourceAmounts.GetOrCreateMatchingPopulationType(populationGain).amount;
        //         return maxPlayerPop;
        //     }
        //
        //     return "not population";
        // }

        public ResourceBundle ScaledResourceBundle()
        {
            if (costScaleResources == null)
            {
                return costRequirements;
            }
            else
            {
                ResourceBundle scaledResourceBundle =
                    new ResourceBundle(costRequirements, (int)costScaleResources.GetScaler());
                if (costScaleResources.ignoreAuthority)
                {
                    scaledResourceBundle.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority).amount
                        = costRequirements.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.Authority).amount;
                }

                if (costScaleResources.ignoreLand)
                {
                    scaledResourceBundle.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.BasicLand).amount
                        = costRequirements.GetOrCreateMatchingResourceLinkType(ResourceType.LinkType.BasicLand).amount;
                }
                return scaledResourceBundle;
            }
        }
        
        
        [MenuItem("Tools/BuildingData")]
        public static void CreateMyAsset()
        {
            
            BuildingData asset = ScriptableObject.CreateInstance<BuildingData>();
            //asset.costRequirements.AddResourceData(GameCenter.instance.resourceOrganizer.CreateResourceData(10, ResourceType.LinkType.Authority));
            AssetDatabase.CreateAsset(asset, "Assets/Data/BuildingData/NewBuilding.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}