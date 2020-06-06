using System;
using System.Threading;
using UnityEngine;

namespace AirHockey.Match.Managers
{
    public class MatchManager : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private PlayerController _leftPlayer;
        [SerializeField] private PlayerController _rightPlayer;
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private PlacementManager _placementManager;
        [SerializeField] private AnnouncementBoard _announcementBoard;
        [SerializeField, Range(0, 10)] private int _matchStartDelay;
        [SerializeField, Range(0, 10)] private int _celebrationDuration;
        [SerializeField, Range(0, 10)] private int _resetDuration;
        [SerializeField, Range(0, 10)] private int _preparationDuration;

        #endregion

        #region Fields

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        #endregion

        #region Setup

        private void Awake()
        {
            Screen.orientation = ScreenOrientation.Landscape;
            _scoreManager.OnScore += HandleScoreAsync;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }
        
        private void Start()
        {
            // TODO: Remove this once the basic game loop is implemented
            StartMatchAsync();
        }

        private void OnDestroy()
        {
            _scoreManager.OnScore -= HandleScoreAsync;
            _cancellationTokenSource.Cancel();
        }

        #endregion

        #region Event handlers

        private async void HandleScoreAsync(Player player)
        {
            try
            {
                _leftPlayer.StopMoving();
                _rightPlayer.StopMoving();
                await _announcementBoard.AnnounceGoalAsync(player, _celebrationDuration * 1_000, _cancellationToken);
                await _placementManager.ResetPlayersAsync(_resetDuration * 1_000, _cancellationToken);
                _placementManager.PlacePuck(player);
                await _announcementBoard.AnnounceGetReadyAsync(_preparationDuration * 1_000, _cancellationToken);
                _leftPlayer.StartMoving();
                _rightPlayer.StartMoving();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Score handling cancelled because the match is over");
            }
        }

        #endregion

        #region Private

        private async void StartMatchAsync()
        {
            try
            {
                _leftPlayer.StopMoving();
                _rightPlayer.StopMoving();
                _placementManager.StartMatch();
                await _announcementBoard.AnnounceMatchStartAsync(_matchStartDelay, _cancellationToken);
                await _announcementBoard.AnnounceGetReadyAsync(_preparationDuration * 1_000, _cancellationToken);
                _leftPlayer.StartMoving();
                _rightPlayer.StartMoving();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Match start cancelled because the match is over");
            }
        }

        #endregion
    }
}