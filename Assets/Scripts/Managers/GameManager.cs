using System;
using AirHockey.Match;
using AirHockey.Match.Managers;
using AirHockey.Menu;
using AirHockey.SceneManagement;
using AirHockey.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AirHockey.Managers
{
    /// <summary>
    /// The top-most manager int he entire application.
    /// </summary>
    internal class GameManager : MonoBehaviour
    {
	    #region Entities

        /// <summary>
        /// Application state.
        /// </summary>
        private enum GamePart
        {
            /// <summary>
            /// Whenever the application is loading for the first time.
            /// </summary>
            None,
            Menu,
            Match
        }
        
        #endregion
        
        #region Serialized fields

        [SerializeField] private SceneReference _menuScene;
        [SerializeField] private SceneReference _matchScene;
        [SerializeField] private CanvasFader _transition;
        [SerializeField] private InputManager _inputManager;

        #endregion

        #region Fields
            
        /// <summary>
        /// The duration of <see cref="UI.Screen"/> transitions in the UI.
        /// </summary>
        private const float TransitionDuration = 1f;
        /// <summary>
        /// The currently loaded scene.
        /// </summary>
        private Scene? _scene;
        private MenuManager _menuManager;
        private MatchManager _matchManager;
        /// <summary>
        /// Current state of the application.
        /// </summary>
        private GamePart _part;
        /// <summary>
        /// Whether a scene is being loaded.
        /// </summary>
        private bool _loading;

        #endregion

        #region Setup

        private async void Start()
        {
            await LoadMenuAsync();
            _inputManager.OnReturn += HandleReturn;
        }

        private void OnDestroy()
        {
            _inputManager.OnReturn -= HandleReturn;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the event of a return from the <see cref="_menuManager"/>.
        /// </summary>
        /// <exception cref="NotImplementedException">Thrown if the current <see cref="GamePart"/>
        /// is invalid.</exception>
        private async void HandleReturn()
        {
            // Ignore the return if it's already loading something.
            if (_loading) 
                return;
            
            switch (_part)
            {
                case GamePart.None:
                    Debug.Log("Can't return when the application is loading.");
                    break;
                case GamePart.Menu:
                    await _menuManager.ReturnAsync();
                    break;
                case GamePart.Match:
                    var matchEnd = _matchManager.StopMatchAsync(TransitionDuration * 0.9f);
                    var loadMenu = LoadMenuAsync();
                    await UniTask.WhenAll(matchEnd, loadMenu);
                    break;
                default:
                    throw new NotImplementedException($"Game part not implemented: {_part}");
            }
        }
        
        /// <summary>
        /// Loads the match <see cref="Scene"/> and starts a new <see cref="Match"/> asynchronously.
        /// </summary>
        /// <param name="settings">The settings of the match to be started.</param>
        private async void LoadMatchAsync(MatchSettings settings)
        {
            _menuManager.OnStartMatch -= LoadMatchAsync;
            _matchManager = await LoadManagedSceneAsync<MatchManager>(_matchScene);
            try
            {
                _part = GamePart.Match;
                await _matchManager.StartMatchAsync(settings);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Match start was cancelled.");
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// Loads the menu scene.
        /// </summary>
        /// <returns>A task to be awaited which represents the loading.</returns>
        private async UniTask LoadMenuAsync()
        {
            _menuManager = await LoadManagedSceneAsync<MenuManager>(_menuScene);
            _menuManager.OnStartMatch += LoadMatchAsync;
            _part = GamePart.Menu;
        }

        /// <summary>
        /// Loads a scene that contains a manager asynchronously.
        /// </summary>
        /// <param name="scene">The scene to be loaded.</param>
        /// <typeparam name="T">The type of the manager to be fetched in the scene.</typeparam>
        /// <returns>A task to be awaited which represents the loading. Its value is the scene's manager. </returns>
        /// <exception cref="Exception">Thrown if the given <paramref name="scene"/> does not contain a manager of type
        /// <typeparamref name="T"/>The type of the manager in the scene.</exception>
        private async UniTask<T> LoadManagedSceneAsync<T>(SceneReference scene) where T : MonoBehaviour
        {
            _loading = true;
            await _transition.FadeInAsync(TransitionDuration);
            if (_scene != null)
                await SceneManager.UnloadSceneAsync(_scene.Value);
            await SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            _scene = SceneManager.GetSceneByPath(scene);
            if (_scene == null)
                throw new Exception($"Managed scene wasn't loaded ({typeof(T)}).");
            
            SceneManager.SetActiveScene(_scene.Value);
            var manager = FindAnyObjectByType<T>();
            await _transition.FadeOutAsync(TransitionDuration);
            _loading = false;
            
            return manager;
        }

        #endregion
    }
}