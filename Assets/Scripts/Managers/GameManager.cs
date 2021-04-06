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
    public class GameManager : MonoBehaviour
    {
        private enum GamePart
        {
            None,
            Menu,
            Match
        }
        
        #region Serialized fields

        [SerializeField] private SceneReference _menuScene;
        [SerializeField] private SceneReference _matchScene;
        [SerializeField] private CanvasFader _transition;
        [SerializeField] private InputManager _inputManager;

        #endregion

        #region Fields

        private const float TransitionDuration = 1f;
        private Scene? _scene;
        private MenuManager _menuManager;
        private MatchManager _matchManager;
        private GamePart _part;
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

        private async void HandleReturn()
        {
            if (_loading) 
                return;
            
            switch (_part)
            {
                case GamePart.None:
                    Debug.Log("Can't return when the application is loading.");
                    break;
                case GamePart.Menu:
                    await _menuManager.Return();
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
        
        private async void LoadMatchAsync(MatchSettings settings)
        {
            _menuManager.OnStartMatch -= LoadMatchAsync;
            _matchManager = await LoadManagedSceneAsync<MatchManager>(_matchScene);
            try
            {
                _part = GamePart.Match;
                await _matchManager.StartMatch(settings);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Match start was cancelled.");
            }
        }

        #endregion

        #region Private

        private async UniTask LoadMenuAsync()
        {
            _menuManager = await LoadManagedSceneAsync<MenuManager>(_menuScene);
            _menuManager.OnStartMatch += LoadMatchAsync;
            _part = GamePart.Menu;
        }

        private async UniTask<TManager> LoadManagedSceneAsync<TManager>(SceneReference scene) 
            where TManager : MonoBehaviour
        {
            _loading = true;
            await _transition.FadeInAsync(TransitionDuration);
            if (_scene != null)
                await SceneManager.UnloadSceneAsync(_scene.Value);
            await SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            _scene = SceneManager.GetSceneByPath(scene);
            if (_scene == null)
                throw new Exception($"Managed scene wasn't loaded ({typeof(TManager)}).");
            
            SceneManager.SetActiveScene(_scene.Value);
            var manager = FindObjectOfType<TManager>();
            await _transition.FadeOutAsync(TransitionDuration);
            _loading = false;
            
            return manager;
        }

        #endregion
    }
}