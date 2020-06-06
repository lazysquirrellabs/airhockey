using UnityEditor;
using UnityEngine;
using AspectRatio = AirHockey.Camera.AspectRatio;

namespace AirHockey.Editor.Camera
{
    [CustomPropertyDrawer(typeof(AspectRatio))]
    public class AspectRatioDrawer : PropertyDrawer
    {
        #region Fields

        private const string HeightName = "_height";
        private const string WidthName = "_width";
        private const int FieldWidth = 25;
        private const int SeparatorWidth = 5;

        #endregion

        #region Update

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var heightFieldWidth = EditorGUIUtility.labelWidth + FieldWidth;
            
            var heightProperty = property.FindPropertyRelative(HeightName);
            var heightPos = position;
            heightPos.width = heightFieldWidth;
            heightProperty.intValue = EditorGUI.IntField(heightPos, label, heightProperty.intValue);
            
            var separatorPos = new Rect(heightPos.xMax, position.y, SeparatorWidth, position.height);
            EditorGUI.LabelField(separatorPos, ":");
            
            var widthProperty = property.FindPropertyRelative(WidthName);
            var widthPos = new Rect(separatorPos.xMax, position.y, FieldWidth, position.height);
            widthProperty.intValue = EditorGUI.IntField(widthPos, widthProperty.intValue);
            
            EditorGUI.EndProperty();
        }
        
        #endregion
    }
}