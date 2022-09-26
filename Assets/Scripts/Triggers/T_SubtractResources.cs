using Data;
using UnityEditor;
using UnityEngine;

namespace Triggers
{
    [System.Serializable]
    public class T_SubtractResources : GameTriggers
    {

        public ResourceBundle resourceBundle;

        public bool scaleWithPlayerAmount = false;
        public ResourceType.LinkType scaleWithLinkType = ResourceType.LinkType.Villager;

        public override void Trigger()
        {
            ResourceBundle activeBundle = isEndOfTurnTrigger
                ? GameCenter.instance.playerBufferResources
                : GameCenter.instance.playerResources;
            if(isEndOfTurnTrigger)
            if (scaleWithPlayerAmount)
            {
                ResourceBundle scaledBundle = new ResourceBundle(resourceBundle, 
                    GameCenter.instance.playerResources.GetOrCreateMatchingResourceLinkType(scaleWithLinkType).amount);
                activeBundle.SubtractResourceBundle(scaledBundle);
            }
            else
            {
                activeBundle.SubtractResourceBundle(resourceBundle);
            }

            EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
        }

        [MenuItem("Tools/Triggers/T_SubtractResources")]
        public static void CreateMyAsset()
        {
            T_SubtractResources asset = ScriptableObject.CreateInstance<T_SubtractResources>();

            AssetDatabase.CreateAsset(asset, "Assets/Data/Triggers/T_SubtractResources.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}