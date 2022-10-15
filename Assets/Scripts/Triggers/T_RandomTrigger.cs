using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Triggers
{
    public class T_RandomTrigger : GameTriggers
    {
        
        [Serializable]
        public struct WeighTriggers
        {
            public WeighTriggers(int weight, GameTriggers trigger)
            {
                this.weight = weight;
                this.triggers = trigger;
            }
            [SerializeField]
            public int weight;
            [SerializeField]
            public GameTriggers triggers;
        }
        
        public List<WeighTriggers> Triggers;
        
        public override void Trigger(StatusIdentifier statusIdentifier = null)
        {
            int total = 0;
            foreach (var weightTrigger in Triggers)
            {
                total += weightTrigger.weight;
            }

            int randomNumber = Random.Range(0, total);
            int sum = 0;
            Debug.Log(total);
            Debug.Log(randomNumber);
            foreach (var weightTrigger in Triggers)
            {
                sum += weightTrigger.weight;
                if (randomNumber < sum)
                {
                    weightTrigger.triggers.Trigger();
                    return;
                }
            }
            //GameCenter.instance.playerResources.AddResource(resourceBundle);
        }

        [MenuItem("Tools/Triggers/RandomTrigger")]
        public static void CreateMyAsset()
        {
            T_RandomTrigger asset = ScriptableObject.CreateInstance<T_RandomTrigger>();

            AssetDatabase.CreateAsset(asset, "Assets/Data/Triggers/RandomTrigger.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}