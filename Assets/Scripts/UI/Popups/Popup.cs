using UnityEngine;
using UnityEngine.UI;

namespace LazySquirrelLabs.AirHockey.UI.Popups
{
	internal abstract class Popup : MonoBehaviour
	{
		#region Serialized fields

		[SerializeField] private Text _text;

		#endregion

		#region Properties

		/// <summary>
		/// Message to be displayed in the popup.
		/// </summary>
		internal string Message
		{
			set => _text.text = value;
		}

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

		#endregion
	}
}