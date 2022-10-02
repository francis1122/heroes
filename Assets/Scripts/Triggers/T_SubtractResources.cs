using Data;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Triggers
{
    [System.Serializable]
    public class T_SubtractResources : GameTriggers
    {

        public ResourceBundle resourceBundle;

        public bool scaleWithPlayerAmount = false;
        public ScaleResources endOfTurnScaleResources = new ScaleResources();
        
        public override void Trigger()
        {
            ResourceBundle activeBundle = isEndOfTurnTrigger
                ? GameCenter.instance.playerBufferResources
                : GameCenter.instance.playerResources;
            if(isEndOfTurnTrigger)
                if (scaleWithPlayerAmount)
                {
                    ResourceBundle scaledBundle = new ResourceBundle(resourceBundle, 
                        (int)endOfTurnScaleResources.GetScaler());
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