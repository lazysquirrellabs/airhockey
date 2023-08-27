using System.Reflection;
using UnityEditor;
using UnityEngine;
using NUnit.Framework;
using AspectRatio = AirHockey.Camera.AspectRatio;

namespace AirHockey.Editor.Camera
{
    [CustomPropertyDrawer(typeof(AspectRatio))]
    internal class AspectRatioDrawer : PropertyDrawer
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
            
            var widthProperty = property.FindPropertyRelative(WidthName);
            var widthPos = position;
            widthPos.width = heightFieldWidth;
            widthProperty.intValue = EditorGUI.IntField(widthPos, label, widthProperty.intValue);
            
            var separatorPos = new Rect(widthPos.xMax, position.y, SeparatorWidth, position.height);
            EditorGUI.LabelField(separatorPos, ":");
            
            var heightProperty = property.FindPropertyRelative(HeightName);
            var heightPos = new Rect(separatorPos.xMax, position.y, FieldWidth, position.height);
            heightProperty.intValue = EditorGUI.IntField(heightPos, heightProperty.intValue);
            
            EditorGUI.EndProperty();
        }
        
        #endregion

        #region Tests

        [Test]
        public void TestFields()
        {
            var type = typeof(AspectRatio);
            var heightField = type.GetField(HeightName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(heightField, "Height field exists.");
            var widthField = type.GetField(WidthName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(widthField, "Width field exists.");
        }

        #endregion
    }
}