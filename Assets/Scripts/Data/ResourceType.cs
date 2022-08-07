using UnityEditor;
using UnityEngine;

namespace Data
{
    public class ResourceType : ScriptableObject
    {
        public string resourceName = "";
        public string resourceShortHand = "";
        public string tempThumbnailResource;
        public Texture UITexture;
        public GameObject dropObject;
        public Sprite dropSprite;
        
        
        
        [MenuItem("Tools/ResourceType")]
        public static void CreateMyAsset()
        {
            ResourceType asset = ScriptableObject.CreateInstance<ResourceType>();

            AssetDatabase.CreateAsset(asset, "Assets/Data/ResourceData/NewResourceType.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
        
    }
}