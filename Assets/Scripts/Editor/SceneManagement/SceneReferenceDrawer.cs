using AirHockey.SceneManagement;
using UnityEditor;
using UnityEngine;

namespace AirHockey.Editor.SceneManagement
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        #region Fields
    
        private const string SceneName = "_scene";
        private const string ScenePathName = "_scenePath";
    
        #endregion
    
        #region Update
    
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var scene = property.FindPropertyRelative(SceneName);
            var scenePath = property.FindPropertyRelative(ScenePathName);
            EditorGUI.BeginChangeCheck();
            var sceneAsset = EditorGUI.ObjectField(position, label, scene.objectReferenceValue, typeof(SceneAsset), false);
            if (EditorGUI.EndChangeCheck() && sceneAsset != null && sceneAsset is SceneAsset asset)
            {
                scenePath.stringValue = AssetDatabase.GetAssetOrScenePath(asset);
                scene.objectReferenceValue = sceneAsset;
            }

            EditorGUI.EndProperty();
        }
    
        #endregion
    }
}