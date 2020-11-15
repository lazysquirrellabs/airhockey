using System;
using AirHockey.Match;
using AirHockey.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Menu {
    public class MenuManager : MonoBehaviour
    {
        #region Events

        public event Action<MatchSettings> OnStartMatch;

        #endregion
        
        #region Serialized fields

        [SerializeField] private Button _newMatchButton;
        [SerializeField] private NewMatchScreen _newMatchScreen;

        #endregion

        #region Fields

        private IDisplayable _currentScreen;

        #endregion

        #region Setup

        private void Awake()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            _newMatchButton.onClick.AddListener(ShowNewMatchScreen);
            _newMatchScreen.OnStartMatch += StartMatch;
        }

        private void OnDestroy()
        {
            _newMatchButton.onClick.RemoveListener(ShowNewMatchScreen);
            _newMatchScreen.OnStartMatch += StartMatch;
        }

        #endregion

        #region Event handlers

        private void ShowNewMatchScreen()
        {
            _newMatchScreen.Show();
            _currentScreen = _newMatchScreen;
        }
        
        private void StartMatch(MatchSettings settings)
        {
            OnStartMatch?.Invoke(settings);
            _currentScreen = null;
        }

        #endregion

        #region Public

        public void Return()
        {
            _currentScreen?.Hide();
            _currentScreen = null;
        }

        #endregion
    }
}