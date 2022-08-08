using Data;
using UnityEditor;
using UnityEngine;

namespace Triggers
{
    [System.Serializable]
    public class T_GenerateResources : GameTriggers
    {

        public ResourceBundle resourceBundle;

        public override void Trigger()
        {
            GameCenter.instance.playerResources.AddResourceBundle(resourceBundle);
            EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
        }

        [MenuItem("Tools/Triggers/GenerateResources")]
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
