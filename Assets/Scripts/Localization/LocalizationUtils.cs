using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace LazySquirrelLabs.AirHockey.Localization
{
	internal static class LocalizationUtils
	{
		#region Internal

		internal static T GetVariable<T>(this LocalizeStringEvent localizer, string key) where T : class, IVariable
		{
			if (localizer.StringReference[key] is T localizedStringVariable)
				return localizedStringVariable;

			return null;
		}

		internal static void Localize(this LocalizeStringEvent localizer, LocalizedString entry)
		{
			localizer.StringReference.TableEntryReference = entry.TableEntryReference;
		}

		#endregion
	}
}