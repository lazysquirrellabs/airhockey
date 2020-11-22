using System;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI
{
    public class Screen : MonoBehaviour
    {
        #region Events

        public event Action OnGoBack;

        #endregion
        
        #region Serialized fields

        [SerializeField] private Button _backButton;

        #endregion

        #region Setup

        protected virtual void Awake()
        {
            _backButton.onClick.AddListener(HandleDismiss);
        }

        protected virtual void OnDestroy()
        {
            _backButton.onClick.RemoveListener(HandleDismiss);
        }

        #endregion

        #region Event handlers

        private void HandleDismiss() => OnGoBack?.Invoke();

        #endregion

        #region Public

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        
        #endregion
    }
}