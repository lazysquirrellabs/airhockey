using System;
using System.Threading;
using AirHockey.Match.Referees;
using AirHockey.Match.Scoring;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AirHockey.Match.Managers
{
    /// <summary>
    /// A <see cref="Match"/>'s manager.
    /// </summary>
    internal class MatchManager : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private PlayerController _leftPlayer;
        [SerializeField] private PlayerController _rightPlayer;
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private PlacementManager _placementManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private AnnouncementBoard _announcementBoard;
        [SerializeField] private Timer _timer;
        [SerializeField, Range(0, 10)] private int _matchStartDelay;
        [SerializeField, Range(0, 10)] private int _celebrationDuration;
        [SerializeField, Range(0, 10)] private int _resetDuration;
        [SerializeField, Range(0, 10)] private int _preparationDuration;

        #endregion

        #region Fields

        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private Referee _referee;
        private Score _score;

        #endregion

        #region Setup

        private void Awake()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            _scoreManager.OnScore += HandleScore;
        }
        
        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _referee?.LeaveMatch(UnsubscribeToScore);
            UnsubscribeToScore(HandleScore);
            
            void UnsubscribeToScore(Scorer scorer) => _scoreManager.OnScore -= scorer;
        }

        #endregion

        #region Event handlers

        private void HandleScore(Player _, Score score)
        {
            _score = score;
            _audioManager.PlayGoal();
        }

        #endregion

        #region Internal

        /// <summary>
        /// Starts a match asynchronously.
        /// </summary>
        /// <param name="settings">The match settings to be used.</param>
        /// <returns>An awaitable task representing the entire match setup process.</returns>
        /// <exception cref="NotImplementedException">Thrown whenever an invalid match <see cref="MatchMode"/> is provided
        /// in the <paramref name="settings"/>.</exception>
        internal async UniTask StartMatchAsync(MatchSettings settings)
        {
            var info = settings.Value;
            _placementManager.StartMatch();
            var token = _cancellationTokenSource.Token;
            await _announcementBoard.AnnounceMatchStartAsync(_matchStartDelay, token);
            await _announcementBoard.AnnounceGetReadyAsync(_preparationDuration, token);
            Debug.Log($"Starting match on {settings.Mode}, value: {info}");
            
            switch (settings.Mode)
            {
                case MatchMode.HighScore:
                    _referee = new HighScoreReferee(ScoreAndResetAsync, End, SubscribeToScore, info);
                    break;
                case MatchMode.BestOfScore:
                    _referee = new BestOfScoreReferee(ScoreAndResetAsync, End, SubscribeToScore, info);
                    break;
                case MatchMode.Time:
                    _timer.Show(info);
                    var seconds = info * 60;
                    var timedReferee = new TimeReferee(ScoreAndResetAsync, End, SubscribeToScore, seconds, _timer.SetTime);
                    timedReferee.StartTimer().Forget();
                    _referee = timedReferee;
                    break;
                case MatchMode.Endless:
                    _referee = new EndlessReferee(ScoreAndResetAsync, End, SubscribeToScore);
                    break;
                default:
                    throw new NotImplementedException($"Mode not implemented: {settings.Mode}");
            }
            
            _audioManager.PlayBuzz();
            _leftPlayer.StartMoving();
            _rightPlayer.StartMoving();
            await _announcementBoard.FadeOutAsync(token);

            void SubscribeToScore(Scorer scorer) => _scoreManager.OnScore += scorer;
        }

        /// <summary>
        /// Forces the match to stop, asynchronously.
        /// </summary>
        /// <param name="fadeOutDuration">How long the stopping should take, in seconds.</param>
        /// <returns>The awaitable task representing the stop process.</returns>
        internal async UniTask StopMatchAsync(float fadeOutDuration)
        {
            _placementManager.StopAll();
            _audioManager.PlayBuzz();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            await _audioManager.FadeOutAllAsync(fadeOutDuration);
        }

        #endregion

        #region Private

        /// <summary>
        /// Scores a goal and resets the rink for the next point, asynchronously.
        /// </summary>
        /// <param name="player">The player who scored</param>
        /// <returns>An awaitable task representing the score announcement and the rink resetting.</returns>
        private async UniTask ScoreAndResetAsync(Player player)
        {
            _leftPlayer.StopMoving();
            _rightPlayer.StopMoving();
            var token = _cancellationTokenSource.Token;
            await _announcementBoard.AnnounceGoalAsync(player, _celebrationDuration, token);
            await _placementManager.ResetPlayersAsync(_resetDuration, token);
            _placementManager.PlacePuck(player);
            await _announcementBoard.AnnounceGetReadyAsync(_preparationDuration, token);
            _audioManager.PlayBuzz();
            _leftPlayer.StartMoving();
            _rightPlayer.StartMoving();
            await _announcementBoard.FadeOutAsync(token);
        }

        /// <summary>
        /// End the match instantly.
        /// </summary>
        private void End()
        {
            Debug.Log("Match is over");
            _placementManager.StopAll();
            _audioManager.PlayBuzz();
            _announcementBoard.AnnounceEndOfMatch(_score.FinalResult, _cancellationTokenSource.Token);
        }

        #endregion
    }
}