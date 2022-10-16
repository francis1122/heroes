using System;
using System.Collections.Generic;
using Triggers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Data
{
    public class BuildingData : ScriptableObject
    {
        public enum BuildingCategory
        {
            Building,
            Action,
            Population
        }
        
        public string buildingName = "";
        [Header("do not edit")]
        public string uniqueName = "";
        public string buildingDetails = "";
        public string buildingThumbnail;

        public ResourceBundle costRequirements;

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
        public bool removeFromPurchasableOnPurchase = true;
        public bool repeatablePurchase = false;
        public bool addToOwnedBuildings = true;

        public List<BuildingData> buildingAdditionsOnPurchase;
        public int priority = 1;

        [SerializeField]
        public List<GameTriggers> onPurchaseTrigger;
        [SerializeField]
        public List<GameTriggers> onTurnEndTrigger;
        [SerializeField]
        public List<GameTriggers> onYearEndTrigger;


        public String GetBuildingCostAndRequirementString()
        {
            String costRequirements = this.costRequirements.GetStringDisplay();
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