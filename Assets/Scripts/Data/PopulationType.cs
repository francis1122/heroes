using System.Collections.Generic;
using Triggers;
using UnityEditor;
using UnityEngine;

namespace Data
{
    public class PopulationType : ScriptableObject
    {
        public enum LinkPopulationType
        {
            Unset = 0,
            // People
            Villager = 100,
            Soldier = 101,
            Prospector = 102,
            Farmer = 103,
            WoodCutter = 104,
            StoneWorker = 105,
            IronWorker = 106,
        }
        
        public LinkPopulationType type = LinkPopulationType.Unset;
        public string populationName = "";
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
        
        
        [MenuItem("Tools/Resources/PopulationType")]
        public static void CreateMyAsset()
        {
            PopulationType asset = CreateInstance<PopulationType>();

            AssetDatabase.CreateAsset(asset, "Assets/Resources/ResourceData/Population/NewPopulationType.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}