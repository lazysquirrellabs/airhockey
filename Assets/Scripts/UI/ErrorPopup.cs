using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI
{
    public class ErrorPopup : Displayable
    {
        #region Serialized fields

        [SerializeField] private Text _text;

        #endregion

        #region Properties

        public string Message
        {
            set => _text.text = value;
        }

        #endregion
    }
}