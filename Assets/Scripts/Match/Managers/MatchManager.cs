using System;
using System.Threading;
using AirHockey.Match.Referees;
using UniRx.Async;
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
        private Referee _referee;

        #endregion

        #region Setup

        private void Awake()
        {
            Screen.orientation = ScreenOrientation.Landscape;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }
        
        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _referee.CancelMatch();
        }

        #endregion

        #region Public

        public async void StartMatch(MatchSettings setting)
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
            
            switch (setting.Mode)
            {
                case Mode.HighScore:
                    _referee = new HighScoreReferee(Pause, ResumeAsync, End, _scoreManager, setting.Value);
                    break;
                case Mode.BestOfScore:
                    _referee = new BestOfScoreReferee(Pause, ResumeAsync, End, _scoreManager, setting.Value);
                    break;
                case Mode.Time:
                    _referee = new TimeReferee(Pause, ResumeAsync, End, _scoreManager, setting.Value);
                    break;
                default:
                    throw new NotImplementedException($"Mode not implemented: {setting.Mode}");
            }
        }

        #endregion

        #region Private

        private void Pause()
        {
            _leftPlayer.StopMoving();
            _rightPlayer.StopMoving();
        }

        private async UniTask ResumeAsync(Player player)
        {
            await _announcementBoard.AnnounceGoalAsync(player, _celebrationDuration * 1_000, _cancellationToken);
            await _placementManager.ResetPlayersAsync(_resetDuration * 1_000, _cancellationToken);
            _placementManager.PlacePuck(player);
            await _announcementBoard.AnnounceGetReadyAsync(_preparationDuration * 1_000, _cancellationToken);
            _leftPlayer.StartMoving();
            _rightPlayer.StartMoving();
        }

        private void End()
        {
            _leftPlayer.StopMoving();
            _rightPlayer.StopMoving();
        }

        #endregion
    }
}