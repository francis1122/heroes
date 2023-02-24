using System.Runtime.InteropServices;
using Data;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Triggers
{
    [System.Serializable]
    public class T_GenerateResources : GameTriggers
    {
        public ResourceBundle resourceBundle = new ResourceBundle();

        public bool scaleWithPlayerAmount = false;
        public ScaleResources endOfTurnScaleResources = new ScaleResources();

        public override bool CanTriggerFire(StatusIdentifier statusIdentifier = null)
        {
            // get building owner

            var bundleToUse = resourceBundle;
            if (scaleWithPlayerAmount)
            {
                bundleToUse = new ResourceBundle(resourceBundle, (int)endOfTurnScaleResources.GetScaler());
            }

            if (isEndOfTurnTrigger)
            {
                GameCenter.instance.ChangePlayerResourcesEndOfTurn(bundleToUse, statusIdentifier);
            }
            else
            {
                return GameCenter.instance.CanChangePlayerResources(bundleToUse, false, statusIdentifier);
            }

            return true;
        }

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
                GameCenter.instance.ChangePlayerResourcesEndOfTurn(bundleToUse, statusIdentifier);
            }
            else
            {
                GameCenter.instance.ChangePlayerResources(bundleToUse, statusIdentifier);
                if (transform != null)
                {
                    Debug.Log("floating text rendering");
                    var floatingText = Instantiate(GameCenter.instance.floatingResourceGameObject,
                        null);
                    //floatingText.transform
                    var localPos = transform.localPosition;
                    var newTra = floatingText.transform.TransformPoint(transform.localPosition);
                    var screenToWorldPosition = Camera.main.ScreenToWorldPoint(transform.position);
                    floatingText.GetComponentInChildren<ResourceFloatingScript>().transform.position =
                        transform.position;
                    floatingText.GetComponentInChildren<ResourceFloatingScript>().InitializeText(bundleToUse);
                }
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