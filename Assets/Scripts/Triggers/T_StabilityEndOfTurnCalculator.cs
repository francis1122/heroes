
using System;
using Data;
using UnityEditor;
using UnityEngine;

namespace Triggers
{
    [System.Serializable]
    public class T_StabilityEndOfTurnCalculator : GameTriggers
    {

        public ResourceBundle resourceBundle;

        [SerializeField]
        public ResourceType stabilityType;
        [SerializeField]
        public ResourceType authorityType;
        
        public override void Trigger()
        {
            ResourceData playerAuthority = GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(authorityType);
            if (playerAuthority.amount < 0)
            {
                double playerAuthF = Math.Abs(playerAuthority.amount);

                int loss = (int)Math.Ceiling((playerAuthF / 10.0f));
                
                
                
                GameCenter.instance.playerResources.SubtractResourceData(new ResourceData( loss, stabilityType));
               //ResourceData playerStabilityType = GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(stabilityType);
                
            }
            //GameCenter.instance.playerResources.AddResourceBundle(resourceBundle);
            EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
        }

        [MenuItem("Tools/Triggers/T_StabilityEndOfTurnCalculator")]
        public static void CreateMyAsset()
        {
            T_StabilityEndOfTurnCalculator asset = ScriptableObject.CreateInstance<T_StabilityEndOfTurnCalculator>();

            AssetDatabase.CreateAsset(asset, "Assets/Data/Triggers/T_StabilityEndOfTurnCalculator.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
