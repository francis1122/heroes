using System.Runtime.InteropServices;
using Data;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Triggers
{
    [System.Serializable]
    public class T_ChangeResourcesLimits : GameTriggers
    {

        public ResourceBundle resourceBundle;

        public bool scaleWithPlayerAmount = false;
        public ScaleResources endOfTurnScaleResources = new ScaleResources();
        
        public override void Trigger(StatusIdentifier statusIdentifier = null, RectTransform transform = null)
        {
            
            
            // get building owner

            var bundleToUse = resourceBundle;
            if (scaleWithPlayerAmount)
            {
                bundleToUse = new ResourceBundle(resourceBundle, (int)endOfTurnScaleResources.GetScaler());
            }

            if (isEndOfTurnTrigger)
            {
                // this probably breaks how the buffer works for end of turn effects
                GameCenter.instance.playerMaxResourceAmounts.AddResourceBundle(bundleToUse);
            }
            else
            {
                GameCenter.instance.playerMaxResourceAmounts.AddResourceBundle(bundleToUse);
            }

            EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
        }

        [MenuItem("Tools/Triggers/T_ChangeResourcesLimits")]
        public static void CreateMyAsset()
        {
            T_ChangeResourcesLimits asset = ScriptableObject.CreateInstance<T_ChangeResourcesLimits>();

            AssetDatabase.CreateAsset(asset, "Assets/Data/Triggers/ChangeResourcesLimits.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
