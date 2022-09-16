using System;
using System.Collections.Generic;
using Triggers;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class ResourceType : ScriptableObject
    {
        
        public enum LinkType
        {
            Unset,
            Authority,
            Gold,
            Food,
            Population,
            Soldier,
            Lumber,
            Ore,
            BasicLand,
            Forest,
            OreDeposit,
            Stability
        }
        
        public enum ResourceCategory
        {
            Material,
            Land,
            People,
            Empire,
            Unique
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
