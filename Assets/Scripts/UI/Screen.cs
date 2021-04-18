using System;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.UI
{
    /// <summary>
    /// Base class for every screen in the game menu.
    /// </summary>
    public class Screen : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// Event invoked whenever this screen wants to go back (return) to the previous one.
        /// </summary>
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

        /// <summary>
        ///  Displays the screen. If it's already displayed, nothing happens.
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Hides the screen. If it's already hidden, nothing happens.
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        
        #endregion
    }
}