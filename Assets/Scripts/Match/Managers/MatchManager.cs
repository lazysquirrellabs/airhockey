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
        [SerializeField] private Timer _timer;
        [SerializeField, Range(0, 10)] private int _matchStartDelay;
        [SerializeField, Range(0, 10)] private int _celebrationDuration;
        [SerializeField, Range(0, 10)] private int _resetDuration;
        [SerializeField, Range(0, 10)] private int _preparationDuration;

        #endregion

        #region Fields

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private Referee _referee;
        private Score _score;

        #endregion

        #region Setup

        private void Awake()
        {
            Screen.orientation = ScreenOrientation.Landscape;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _scoreManager.OnScore += HandleScore;
        }
        
        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _referee.CancelMatch(UnsubscribeToScore);
            UnsubscribeToScore(HandleScore);
            
            void UnsubscribeToScore(Scorer scorer) => _scoreManager.OnScore -= scorer;
        }

        #endregion

        #region Event handlers

        private void HandleScore(Player _, Score score) => _score = score;

        #endregion

        #region Public

        public async void StartMatch(MatchSettings setting)
        {
            var info = setting.Value;
            Debug.Log($"Starting match on {setting.Mode}, value: {info}");
            _leftPlayer.StopMoving();
            _rightPlayer.StopMoving();
            _placementManager.StartMatch();
            
            try
            {
                await _announcementBoard.AnnounceMatchStartAsync(_matchStartDelay, _cancellationToken);
                await _announcementBoard.AnnounceGetReadyAsync(_preparationDuration * 1_000, _cancellationToken);
                switch (setting.Mode)
                {
                    case Mode.HighScore:
                        _referee = new HighScoreReferee(PauseAsync, End, SubscribeToScore, info);
                        break;
                    case Mode.BestOfScore:
                        _referee = new BestOfScoreReferee(PauseAsync, End, SubscribeToScore, info);
                        break;
                    case Mode.Time:
                        _timer.Show(info);
                        _referee = new TimeReferee(PauseAsync, End, SubscribeToScore, info, _timer.SetTime);
                        break;
                    case Mode.Endless:
                        _referee = new EndlessReferee(PauseAsync, End, SubscribeToScore);
                        break;
                    default:
                        throw new NotImplementedException($"Mode not implemented: {setting.Mode}");
                }
                _leftPlayer.StartMoving();
                _rightPlayer.StartMoving();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Match start cancelled because the match is over");
            }

            void SubscribeToScore(Scorer scorer) => _scoreManager.OnScore += scorer;
        }

        #endregion

        #region Private

        private async UniTask PauseAsync(Player player)
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

        private void End()
        {
            Debug.Log("Match is over");
            _leftPlayer.StopMoving();
            _rightPlayer.StopMoving();
            _announcementBoard.AnnounceEndOfMatch(_score.FinalResult, _cancellationToken);
        }

        #endregion
    }
}