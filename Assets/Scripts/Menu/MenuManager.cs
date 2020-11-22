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
            _playButton.onClick.AddListener(_newMatchScreen.Show);
            _settingsButton.onClick.AddListener(_settingsScreen.Show);
            _creditsButton.onClick.AddListener(_creditsScreen.Show);
            _newMatchScreen.OnStartMatch += StartMatch;
            _settingsScreen.LoadAudioLevels();
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(_newMatchScreen.Show);
            _settingsButton.onClick.RemoveListener(_settingsScreen.Show);
            _creditsButton.onClick.RemoveListener(_creditsScreen.Show);
            _newMatchScreen.OnStartMatch -= StartMatch;
        }

        #endregion

        #region Event handlers

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
    }
}