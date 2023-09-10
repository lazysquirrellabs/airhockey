using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI.Popups
{
    /// <summary>
    /// A simple popup with a message and an acknowledgment button.
    /// </summary>
    internal class MessagePopup : Popup
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