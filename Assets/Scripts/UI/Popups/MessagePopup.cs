using System;
using UnityEngine;
using UnityEngine.UI;

namespace LazySquirrelLabs.AirHockey.UI.Popups
{
    /// <summary>
    /// A simple popup with a message and an acknowledgment button.
    /// </summary>
    internal class MessagePopup : Popup
    {
	    #region Events

	    internal event Action OnAcknowledge;

	    #endregion
	    
        #region Serialized fields

        [SerializeField] private Button _acknowledgeButton;

        #endregion

        #region Setup

        private void Awake()
        {
	        _acknowledgeButton.onClick.AddListener(HandleAcknowledge);
        }

        private void OnDestroy()
        {
	        _acknowledgeButton.onClick.RemoveListener(HandleAcknowledge);
        }

        #endregion

        #region Event handlers

        private void HandleAcknowledge()
        {
	        Hide();
	        OnAcknowledge?.Invoke();
        }

        #endregion
    }
}