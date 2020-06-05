using System;
using AirHockey.Match.Managers;
using AirHockey.Menu;
using AirHockey.SceneManagement;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AirHockey.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private SceneReference _menuScene;
        [SerializeField] private SceneReference _matchScene;

        #endregion

        #region Fields
        
        private Scene? _scene;

        private MenuManager _menuManager;
        private MatchManager _matchManager;

        #endregion

        #region Setup

        private void Start()
        {
            LoadMenuAsync();
        }

        #endregion

        #region Private

        private async void LoadMenuAsync()
        {
            _menuManager = await LoadManagedSceneAsync<MenuManager>(_menuScene);
            _menuManager.OnStartMatch += LoadMatchAsync;
        }

        private async void LoadMatchAsync()
        {
            _menuManager.OnStartMatch -= LoadMatchAsync;
            _matchManager = await LoadManagedSceneAsync<MatchManager>(_matchScene);
        }

        private async UniTask<TManager> LoadManagedSceneAsync<TManager>(SceneReference scene) 
            where TManager : MonoBehaviour
        {
            if (_scene != null)
                await SceneManager.UnloadSceneAsync(_scene.Value);
            await SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            _scene = SceneManager.GetSceneByPath(scene);
            if (_scene == null)
                throw new Exception($"Managed scene wasn't loaded ({typeof(TManager)}).");
            
            SceneManager.SetActiveScene(_scene.Value);
            return FindObjectOfType<TManager>();
        }

        #endregion
    }
}