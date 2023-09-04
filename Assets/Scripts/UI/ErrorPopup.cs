using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI
{
    /// <summary>
    /// Error popup with a message.
    /// </summary>
    internal class ErrorPopup : Screen
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
    }
}