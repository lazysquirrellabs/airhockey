using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI.Popups
{
    /// <summary>
    /// Error popup with a message.
    /// </summary>
    internal class ErrorPopup : Popup
    {
        #region Serialized fields

        [SerializeField] private Button _acknowledgeButton;

        #endregion


        #region Setup

        private void Awake()
        {
	        _acknowledgeButton.onClick.AddListener(Hide);
        }

        private void OnDestroy()
        {
	        _acknowledgeButton.onClick.RemoveListener(Hide);
        }

        #endregion
    }
}