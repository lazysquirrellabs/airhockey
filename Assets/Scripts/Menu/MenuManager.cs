using System;
using AirHockey.Match;
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

        #region Setup

        private void Awake()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            _newMatchButton.onClick.AddListener(_newMatchScreen.Show);
            _newMatchScreen.OnStartMatch += StartMatch;
        }

        private void OnDestroy()
        {
            _newMatchButton.onClick.RemoveListener(_newMatchScreen.Show);
            _newMatchScreen.OnStartMatch += StartMatch;
        }

        #endregion

        #region Event handlers

        private void StartMatch(MatchSettings settings)
        {
            OnStartMatch?.Invoke(settings);
        }

        #endregion
    }
}