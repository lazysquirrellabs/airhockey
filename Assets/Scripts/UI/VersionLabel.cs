using LazySquirrelLabs.AirHockey.Localization;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace LazySquirrelLabs.AirHockey.UI
{
	/// <summary>
	/// Sets a Text label with the application version.
	/// </summary>
	internal class VersionLabel : MonoBehaviour
	{
		#region Serialized fields

		[SerializeField] private LocalizeStringEvent _localizes;
		[SerializeField] private Text _label;

		#endregion

		#region Fields

		private const string VariableKey = "version";

		#endregion

		#region Setup

		private void Start()
		{
			var parameter = _localizes.GetVariable<StringVariable>(VariableKey);
			parameter.Value = Application.version;
		}

		#endregion
	}
}