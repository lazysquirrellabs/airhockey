using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace LazySquirrelLabs.AirHockey.UI.Localization
{
	internal class LanguageDropdown : MonoBehaviour
	{
		[Serializable]
		private struct LanguageEntry
		{
			#region Serialized field

			[SerializeField] private Locale _locale;
			[SerializeField] private string _label;

			#endregion

			#region Properties

			internal Locale Locale => _locale;
			internal string Label => _label;

			#endregion
		}
		
		#region Serialized fields

		[SerializeField] private LocalizationSettings _localizationSettings;
		[SerializeField] private Dropdown _dropdown;
		[SerializeField] private LanguageEntry[] _languageEntries;

		#endregion
		
		#region Setup

		internal void Awake()
		{
			_dropdown.onValueChanged.AddListener(HandleSelectLanguage);
			var options = _languageEntries.Select(l => l.Label).ToList();
			_dropdown.ClearOptions();
			_dropdown.AddOptions(options);
			var selectedLocale = _localizationSettings.GetSelectedLocale();
			var selectedIndex = Array.FindIndex(_languageEntries,
			                                    e => e.Locale.Identifier == selectedLocale.Identifier);
			_dropdown.value = selectedIndex;
		}

		#endregion
		
		#region Event handlers

		private void HandleSelectLanguage(int index)
		{
			var locale = _languageEntries[index].Locale;
			_localizationSettings.SetSelectedLocale(locale);
		}

		#endregion


	}
}