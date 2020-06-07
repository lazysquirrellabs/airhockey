using System;
using AirHockey.Match;
using AirHockey.UI.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Menu
{
    public class NewMatchScreen : MonoBehaviour
    {
        #region Events

        public event Action<MatchSettings> OnStartMatch;

        #endregion
        
        #region Serialized fields

        [SerializeField] private ModeToggleGroup _modeSelector;
        [SerializeField] private Button _startButton;

        #endregion

        #region Setup

        private void Awake()
        {
            _startButton.onClick.AddListener(HandleStart);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(HandleStart);
        }

        #endregion

        #region Event handlers

        private void HandleStart()
        {
            // TODO: replace by value toggle
            uint value = 5;
            var settings = new MatchSettings(_modeSelector.Selected, value);
            OnStartMatch?.Invoke(settings);
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