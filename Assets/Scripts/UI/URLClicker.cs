using UnityEngine;
using UnityEngine.EventSystems;

namespace LazySquirrelLabs.AirHockey.UI
{
	/// <summary>
	/// Opens a URL whenever the pointer clicks on this component.
	/// </summary>
	internal class URLClicker : MonoBehaviour, IPointerClickHandler
	{
		#region Serialized fields

		[SerializeField] private string _url;

		#endregion

		#region Event handlers

		public void OnPointerClick(PointerEventData eventData)
		{
			Application.OpenURL(_url);
		}

		#endregion
	}
}