using System.Reflection;
using LazySquirrelLabs.AirHockey.SceneManagement;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace LazySquirrelLabs.AirHockey.Editor.SceneManagement
{
	[CustomPropertyDrawer(typeof(SceneReference))]
	internal class SceneReferenceDrawer : PropertyDrawer
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
			var sceneAsset = EditorGUI.ObjectField(position, label, scene.objectReferenceValue, typeof(SceneAsset),
			                                       false);

			if (EditorGUI.EndChangeCheck() && sceneAsset != null && sceneAsset is SceneAsset asset)
			{
				scenePath.stringValue = AssetDatabase.GetAssetOrScenePath(asset);
				scene.objectReferenceValue = sceneAsset;
			}

			EditorGUI.EndProperty();
		}

		#endregion

		#region Tests

		[Test]
		public void TestFields()
		{
			var type = typeof(SceneReference);
			var sceneField = type.GetField(SceneName, BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(sceneField, "Scene field exists.");
			var scenePathField = type.GetField(ScenePathName, BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(scenePathField, "Scene patch field exists.");
		}

		#endregion
	}
}