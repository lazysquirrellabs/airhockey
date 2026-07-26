using LazySquirrelLabs.AirHockey.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace LazySquirrelLabs.AirHockey.UI.Popups
{
	internal abstract class Popup : MonoBehaviour
	{
		#region Serialized fields

		[SerializeField] private LocalizeStringEvent _localizer;

		#endregion

		#region Fields

		private LocalizedString _localizedVariable;
		private StringVariable _stringVariable;

		#endregion

		#region Internal

		internal void Show()
		{
			gameObject.SetActive(true);
		}

		internal void Hide()
		{
			gameObject.SetActive(false);
		}

		internal void SetMessage(LocalizedString messageEntry)
		{
			_localizer.Localize(messageEntry);
		}
		
		internal void SetMessageWithVariable(LocalizedString messageEntry, LocalizedString variableEntry)
		{
			InitializeVariables();
			SetMessage(messageEntry);
			_localizedVariable.TableReference = variableEntry.TableReference;
			_localizedVariable.TableEntryReference = variableEntry.TableEntryReference;
		}
		
		internal void SetMessageWithText(LocalizedString messageEntry, string text)
		{
			InitializeVariables();
			SetMessage(messageEntry);
			_stringVariable.Value = text;
		}

		#endregion

		#region Private

		private void InitializeVariables()
		{
			_localizedVariable ??= _localizer.GetVariable<LocalizedString>("value");
			_stringVariable ??= _localizer.GetVariable<StringVariable>("string-value");
		}

		#endregion
	}
}