using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI
{
    public class ErrorPopup : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private Text _text;
        [SerializeField] private Button _button;

        #endregion

        #region Setup

        private void Awake()
        {
            _button.onClick.AddListener(Hide);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(Hide);
        }

        #endregion

        #region Event handlers

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Public

        public void Show(string message)
        {
            _text.text = message;
            gameObject.SetActive(true);
        }

        #endregion
    }
}