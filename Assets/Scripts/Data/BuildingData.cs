using System;
using System.Collections.Generic;
using Triggers;
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
        public int amount;
        public ResourceType type;

        
        public BuildingCategory category = BuildingCategory.Building;
        public BuildingBundle[] buildingRequirement = Array.Empty<BuildingBundle>();
        public bool destroyRequiredBuildings = false;

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
        
        [MenuItem("Tools/BuildingData")]
        public static void CreateMyAsset()
        {
            BuildingData asset = ScriptableObject.CreateInstance<BuildingData>();

            AssetDatabase.CreateAsset(asset, "Assets/Data/BuildingData/NewBuilding.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}