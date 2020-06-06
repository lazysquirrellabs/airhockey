using System;
using AirHockey.Match;
using AirHockey.Match.Managers;
using AirHockey.Menu;
using AirHockey.SceneManagement;
using AirHockey.UI;
using UniRx.Async;
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
        
        private Scene? _scene;
        private MenuManager _menuManager;
        private MatchManager _matchManager;
        private GamePart _part;
        private bool _loading;

        #endregion

        #region Setup

        private void Start()
        {
            LoadMenuAsync();
            _inputManager.OnReturn += HandleReturn;
        }

        private void OnDestroy()
        {
            _inputManager.OnReturn -= HandleReturn;
        }

        #endregion

        #region Event handlers

        private void HandleReturn()
        {
            if (_loading) return;
            
            switch (_part)
            {
                case GamePart.None:
                    Debug.Log("Can't return when the application is loading.");
                    break;
                case GamePart.Menu:
                    // TODO: Handle menu return
                    break;
                case GamePart.Match:
                    LoadMenuAsync();
                    break;
                default:
                    throw new NotImplementedException($"Game part not implemented: {_part}");
            }
        }

        #endregion

        #region Private

        private async void LoadMenuAsync()
        {
            _menuManager = await LoadManagedSceneAsync<MenuManager>(_menuScene);
            _menuManager.OnStartMatch += LoadMatchAsync;
            _part = GamePart.Menu;
        }

        private async void LoadMatchAsync()
        {
            _menuManager.OnStartMatch -= LoadMatchAsync;
            _matchManager = await LoadManagedSceneAsync<MatchManager>(_matchScene);
            _matchManager.StartMatch(new MatchSettings(Mode.HighScore, 3));
            _part = GamePart.Match;
        }

        private async UniTask<TManager> LoadManagedSceneAsync<TManager>(SceneReference scene) 
            where TManager : MonoBehaviour
        {
            _loading = true;
            await _transition.FadeInAsync();
            if (_scene != null)
                await SceneManager.UnloadSceneAsync(_scene.Value);
            await SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            _scene = SceneManager.GetSceneByPath(scene);
            if (_scene == null)
                throw new Exception($"Managed scene wasn't loaded ({typeof(TManager)}).");
            
            SceneManager.SetActiveScene(_scene.Value);
            var manager = FindObjectOfType<TManager>();
            await _transition.FadeOutAsync();
            _loading = false;
            
            return manager;
        }

        #endregion
    }
}