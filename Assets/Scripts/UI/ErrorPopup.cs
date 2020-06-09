using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI
{
    public class ErrorPopup : MonoBehaviour, IDisplayable
    {
        #region Serialized fields

        [SerializeField] private Text _text;
        [SerializeField] private Button _button;

        #endregion

        #region Properties

        public string Message
        {
            set => _text.text = value;
        }

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

        #region Public

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}