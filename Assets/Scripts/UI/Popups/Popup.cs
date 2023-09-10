using UnityEngine;

namespace AirHockey.UI.Popups
{
	internal abstract class Popup : MonoBehaviour
	{
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