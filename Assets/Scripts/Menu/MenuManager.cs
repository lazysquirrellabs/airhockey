using System;
using AirHockey.Match;
using AirHockey.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Menu 
{
    public class MenuManager : MonoBehaviour
    {
        #region Events

        public event Action<MatchSettings> OnStartMatch;

        #endregion
        
        #region Serialized fields

        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private NewMatchScreen _newMatchScreen;
        [SerializeField] private SettingsScreen _settingsScreen;
        [SerializeField] private Displayable _creditsScreen;

        #endregion

        #region Fields

        private Displayable _currentScreen;

        #endregion

        #region Setup

        private void Awake()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            _playButton.onClick.AddListener(HandleShowStartScreen);
            _settingsButton.onClick.AddListener(HandleShowSettingsScreen);
            _creditsButton.onClick.AddListener(HandleShowCreditsScreen);
            _newMatchScreen.OnStartMatch += StartMatch;
            _settingsScreen.LoadAudioLevels();
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(HandleShowStartScreen);
            _settingsButton.onClick.RemoveListener(HandleShowSettingsScreen);
            _creditsButton.onClick.RemoveListener(HandleShowCreditsScreen);
            _newMatchScreen.OnStartMatch -= StartMatch;
        }

        #endregion

        #region Event handlers

        private void HandleShowStartScreen()
        {
            ShowScreen(_newMatchScreen);
        }
        
        private void HandleShowSettingsScreen()
        {
            ShowScreen(_settingsScreen);
        }
        
        private void HandleShowCreditsScreen()
        {
            ShowScreen(_creditsScreen);
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
            if (_currentScreen != null)
                _currentScreen.Hide();
            _currentScreen = null;
        }

        #endregion

        #region Private
        
        private void ShowScreen(Displayable screen)
        {
            if (_currentScreen != null)
                _currentScreen.Hide();
            _currentScreen = screen;
            _currentScreen.Show();
        }

        #endregion
    }
}