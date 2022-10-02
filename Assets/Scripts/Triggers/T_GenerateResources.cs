using Data;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Triggers
{
    [System.Serializable]
    public class T_GenerateResources : GameTriggers
    {

        public ResourceBundle resourceBundle;

        public bool scaleWithPlayerAmount = false;
        public ScaleResources endOfTurnScaleResources = new ScaleResources();
        
        public override void Trigger()
        {
            
            ResourceBundle activeBundle = isEndOfTurnTrigger
                ? GameCenter.instance.playerBufferResources
                : GameCenter.instance.playerResources;
            if (scaleWithPlayerAmount)
            {
                ResourceBundle scaledBundle = new ResourceBundle(resourceBundle, (int)endOfTurnScaleResources.GetScaler());
                activeBundle.AddResourceBundle(scaledBundle);
            }
            else
            {
                activeBundle.AddResourceBundle(resourceBundle);
            }

            EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
        }

        [MenuItem("Tools/Triggers/T_GenerateResources")]
        public static void CreateMyAsset()
        {
            T_GenerateResources asset = ScriptableObject.CreateInstance<T_GenerateResources>();

            AssetDatabase.CreateAsset(asset, "Assets/Data/Triggers/GenerateResources.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
