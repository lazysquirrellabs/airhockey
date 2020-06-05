using System;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Menu {
    public class MenuManager : MonoBehaviour
    {
        #region Events

        public event Action OnStartMatch;

        #endregion
        
        #region Serialized fields

        [SerializeField] private Button _startButton;

        #endregion

        #region Setup

        private void Awake()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            _startButton.onClick.AddListener(StartMatch);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(StartMatch);
        }

        #endregion

        #region Event handlers

        private void StartMatch()
        {
            OnStartMatch?.Invoke();
        }

        #endregion
    }
}