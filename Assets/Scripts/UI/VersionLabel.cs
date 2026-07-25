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

		[SerializeField] private LocalizeStringEvent _localizeStringEvent;
		[SerializeField] private Text _label;

		#endregion

		#region Fields

		private const string VariableKey = "version";

		#endregion

		#region Setup

		private void Start()
		{
			if (_localizeStringEvent.StringReference[VariableKey] is StringVariable parameter)
				parameter.Value = Application.version;
			else
				Debug.LogWarning($"Version label's variable key ({VariableKey}) not found.");
		}

		#endregion
	}
}