using System;
using System.Collections.Generic;
using Triggers;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Data
{
    [System.Serializable]
    public class ResourceType : ScriptableObject
    {
        
        public enum LinkType
        {
            Unset = 0,
            Authority = 1,
            //Material
            Gold = 200,
            Food = 201,
            Lumber = 202,
            Ore = 203,
            Iron = 204,
            // Lands
            BasicLand = 300,
            Forest = 301,
            OreDeposit = 302,
            IronDeposit = 303,
            
            // Empire
            Stability = 400,
            MaxPopulation = 401,
            MilitaryPower = 402,
            Happiness = 403
        }
        
        public enum ResourceCategory
        {
            Unset = 0,
            Material = 1,
            Land = 2,
            Empire = 3,
            Unique = 4,
            
        }

        public LinkType type = LinkType.Unset;
        public string resourceName = "";
        public string resourceShortHand = "";
        public string tempThumbnailResource;
        public int UIPriority;
        public Texture UITexture;
        public GameObject dropObject;
        public Sprite dropSprite;

        public bool amountCanBeNegative = false;
        public bool checkForPlayerResourceMaxLimit = false;
        public bool checkForPlayerResourceMinLimit = false;
        public List<GameTriggers> playerEndOfTurnTriggers;
        

        [SerializeField] public ResourceCategory resourceCategory = ResourceCategory.Material;
        
        [MenuItem("Tools/ResourceType")]
        public static void CreateMyAsset()
        {
            ResourceType asset = ScriptableObject.CreateInstance<ResourceType>();

            AssetDatabase.CreateAsset(asset, "Assets/Resources/ResourceData/NewResourceType.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
