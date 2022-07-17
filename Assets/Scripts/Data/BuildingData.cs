using System.Collections.Generic;
using Triggers;
using UnityEditor;
using UnityEngine;

namespace Data
{
    public class BuildingData : ScriptableObject
    {
        public string buildingName = "";
        public string buildingDetails = "";
        public string buildingThumbnail;

        public ResourceBundle costRequirements;
        public BuildingData buildingRequirement;

        public bool removeFromPurchasableOnPurchase = true;
        public bool repeatablePurchase = false;
        public bool addToOwnedBuildings = true;

        public List<BuildingData> buildingAdditionsOnPurchase;
        public int priority = 1;

        [SerializeField]
        public List<GameTriggers> onPurchaseTrigger;
        public List<GameTriggers> onTurnEndTrigger;
        
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