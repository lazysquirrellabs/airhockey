using System;
using System.Threading;
using LazySquirrelLabs.AirHockey.Match;
using LazySquirrelLabs.AirHockey.UI;
using LazySquirrelLabs.AirHockey.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Screen = LazySquirrelLabs.AirHockey.UI.Screen;

namespace LazySquirrelLabs.AirHockey.Menu 
{
    /// <summary>
    /// Manages the game's main menu.
    /// </summary>
    internal class MenuManager : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// Invoked whenever a new match starts.
        /// </summary>
        internal event Action<MatchSettings> OnStartMatch;

        /// <summary>
        /// Invoked whenever the application leaves a submenu and goes back to the main menu.
        /// </summary>
        internal event Action OnEnterMenu;

        /// <summary>
        /// Invoked whenever the application goes to a submenu.
        /// </summary>
        internal event Action OnReturnToMainMenu;

        #endregion
        
        #region Serialized fields

        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private PlayScreen _playScreen;
        [SerializeField] private SettingsScreen _settingsScreen;
        [SerializeField] private Screen _creditsScreen;
        [SerializeField] private CanvasFader _transition;
        [SerializeField, Range(0, 10)] private float _transitionDuration;

        #endregion

        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        #region Fields

        private Screen _currentScreen;

        #endregion

        #region Setup

        private void Awake()
        {
            UnityEngine.Screen.orientation = ScreenOrientation.Portrait;
            _playButton.onClick.AddListener(HandleSelectNewMatch);
            _settingsButton.onClick.AddListener(HandleSelectSettings);
            _creditsButton.onClick.AddListener(HandleSelectCredits);
            _playScreen.OnGoBack += HandleReturn;
            _settingsScreen.OnGoBack += HandleReturn;
            _creditsScreen.OnGoBack += HandleReturn;
            _playScreen.OnStartMatch += StartMatch;
            _settingsScreen.LoadAudioLevels();
        }

        private void OnDestroy()
        {
	        _cancellationTokenSource.Cancel();
	        _cancellationTokenSource.Dispose();
            _playButton.onClick.RemoveListener(HandleSelectNewMatch);
            _settingsButton.onClick.RemoveListener(HandleSelectSettings);
            _creditsButton.onClick.RemoveListener(HandleSelectCredits);
            _playScreen.OnGoBack -= HandleReturn;
            _settingsScreen.OnGoBack -= HandleReturn;
            _creditsScreen.OnGoBack -= HandleReturn;
            _playScreen.OnStartMatch -= StartMatch;
        }

        #endregion

        #region Event handlers

        private async void HandleSelectNewMatch()
        {
	        OnEnterMenu?.Invoke();
	        try
	        {
		        await TransitionToAsync(_playScreen);
	        }
	        catch (OperationCanceledException)
	        {
		        Debug.Log("New match selection handling stopped because the operation was cancelled.");
	        }
        }
        
        private async void HandleSelectSettings()
        {
	        OnEnterMenu?.Invoke();
	        try
	        {
		        await TransitionToAsync(_settingsScreen);
	        }
	        catch (OperationCanceledException)
	        {
		        Debug.Log("Settings selection handling stopped because the operation was cancelled.");
	        }
        }
        
        private async void HandleSelectCredits()
        {
	        OnEnterMenu?.Invoke();
	        try
	        {
		        await TransitionToAsync(_creditsScreen);
	        }
	        catch (OperationCanceledException)
	        {
		        Debug.Log("Credits selection handling stopped because the operation was cancelled.");
	        }
        }
        
        private void StartMatch(MatchSettings settings)
        {
            OnStartMatch?.Invoke(settings);
            _currentScreen = null;
        }

        private async void HandleReturn()
        {
	        try
	        {
		        await ReturnMenuAsync(_cancellationTokenSource.Token);
	        }
	        catch (OperationCanceledException)
	        {
		        Debug.Log("Stopped handling menu return because the operation was cancelled.");
	        }
        }

        #endregion

        #region Internal
        
        internal async UniTask ReturnAsync(CancellationToken token)
        {
	        var unifiedToken = token.Unify(_cancellationTokenSource.Token);
	        await ReturnMenuAsync(unifiedToken);
        }

        #endregion

        #region Private

        private async UniTask ReturnMenuAsync(CancellationToken token)
        {
	        if (_currentScreen == null)
		        return;
	        
	        await _transition.FadeInAsync(_transitionDuration / 2f, token);
		    _currentScreen.Hide();
	        _currentScreen = null;
	        await _transition.FadeOutAsync(_transitionDuration / 2f, token);
	        OnReturnToMainMenu?.Invoke();
        }
        
        /// <summary>
        /// Transitions to the given <paramref name="screen"/> asynchronously.
        /// </summary>
        /// <param name="screen">The screen to transition to.</param>
        /// <returns>A task to be awaited representing the fade transition.</returns>
        private async UniTask TransitionToAsync(Screen screen)
        {
            await _transition.FadeInAsync(_transitionDuration / 2f, _cancellationTokenSource.Token);
           
            if (_currentScreen != null)
                _currentScreen.Hide();
           
            screen.Show();
            _currentScreen = screen;
            
            await _transition.FadeOutAsync(_transitionDuration / 2f, _cancellationTokenSource.Token);
        }

        #endregion
    }
}