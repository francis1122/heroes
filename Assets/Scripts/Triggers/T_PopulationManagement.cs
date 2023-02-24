using System;
using Data;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Triggers
{
    public class T_PopulationManagement: GameTriggers
    {

        [SerializeField]
        public ResourceType populationType;
        [SerializeField]
        public ResourceType foodType;


                
        public override void Trigger(StatusIdentifier statusIdentifier = null, RectTransform transform = null)
        {
            ResourceData playerPopulation = GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(populationType);
            ResourceData playerFood = GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(foodType);
            if (playerFood.amount < playerPopulation.amount)
            {
                /*
                double playerAuthF = Math.Abs(playerAuthority.amount);

                int loss = (int)Math.Ceiling((playerAuthF / 10.0f));
                
                
                
                GameCenter.instance.playerResources.SubtractResourceData(new ResourceData( loss, stabilityType));
                //ResourceData playerStabilityType = GameCenter.instance.playerResources.GetOrCreateMatchingResourceType(stabilityType);
                */
                
            }
            else
            {
                
                GameCenter.instance.playerResources.SubtractResourceData(new ResourceData( playerPopulation.amount, foodType));
            }
            //GameCenter.instance.playerResources.AddResourceBundle(resourceBundle);
            EventManager.TriggerEvent(EventManager.RESOURCES_CHANGED);
        }
        
        
        [MenuItem("Tools/Triggers/Managers/T_PopulationManagement")]
        public static void CreateMyAsset()
        {
            T_PopulationManagement asset = ScriptableObject.CreateInstance<T_PopulationManagement>();

            AssetDatabase.CreateAsset(asset, "Assets/Data/Triggers/Managers/T_PopulationManagement.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}