
using System;
using Data;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Triggers
{
    [System.Serializable]
    public class T_StabilityEndOfTurnCalculator : GameTriggers
    {
        
        public override void Trigger(StatusIdentifier statusIdentifier = null)
        {
            
            ResourceData playerAuthority = GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(GameCenter.instance.resourceOrganizer.GetResourceType(ResourceType.LinkType.Authority));
            if (playerAuthority.amount < 0)
            {
                double playerAuthF = Math.Abs(playerAuthority.amount);

                int loss = (int)Math.Ceiling((playerAuthF / 10.0f));
                
                
                
                GameCenter.instance.playerResources.SubtractResourceData(GameCenter.instance.resourceOrganizer.CreateResourceData(loss, ResourceType.LinkType.Stability));
               //ResourceData playerStabilityType = GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(stabilityType);
                
            }
            //GameCenter.instance.playerResources.AddResourceBundle(resourceBundle);
            EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
        }

        [MenuItem("Tools/Triggers/Managers/T_StabilityEndOfTurnCalculator")]
        public static void CreateMyAsset()
        {
            T_StabilityEndOfTurnCalculator asset = ScriptableObject.CreateInstance<T_StabilityEndOfTurnCalculator>();

            AssetDatabase.CreateAsset(asset, "Assets/Data/Triggers/Managers/T_StabilityEndOfTurnCalculator.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
