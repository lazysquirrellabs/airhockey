using System;
using System.Threading;
using AirHockey.Match.Scoring;
using AirHockey.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniTaskExtensions = AirHockey.Utils.UniTaskExtensions;

namespace AirHockey.Match
{
    /// <summary>
    /// Board seen in the match which gives general visual announcements (e.g. score, match start and end).
    /// </summary>
    internal class AnnouncementBoard : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private CanvasGroup _canvas;
        [SerializeField] private Text _leftText;
        [SerializeField] private Text _rightText;

        #endregion

        #region Fields

        /// <summary> Duration in seconds of general fade outs used in FadeOutAsync. </summary>
        private const float FadeOutDuration = 1f;
        private const float MatchStartFadeDuration = 0.5f;
        private const float MatchEndFadeDuration = 0.5f;
        private const string MatchStartText = "MATCH STARTS IN {0}...";
        private const string ScoredText = "GOAL!!!";
        private const string OtherScoreText = "PLAYER {0} SCORED";
        private const string GetReadyText = "ON YOUR MARKS...";
        private const string GoText = "GO!";
        private const string YouWinText = "YOU WIN!!";
        private const string YouLoseText = "YOU LOSE";
        private const string TieText = "IT'' A TIE";

        #endregion

        #region Fields

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        #endregion

        #region Setup

        private void OnDestroy()
        {
	        _cancellationTokenSource.Cancel();
	        _cancellationTokenSource.Dispose();
        }

        #endregion

        #region Internal

        /// <summary>
        /// Displays a "match is starting..." announcement asynchronously.
        /// </summary>
        /// <param name="duration">The duration of the announcement.</param>
        /// <param name="token">The token for operation cancellation.</param>
        /// <returns>The awaitable task.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="duration"/>
        /// is negative.</exception>
        internal async UniTask AnnounceMatchStartAsync(int duration, CancellationToken token)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration must be positive.");
            
            SetTexts(duration);
            var unifiedToken = token.Unify(_cancellationTokenSource.Token);
            await FadeInAsync(MatchStartFadeDuration, unifiedToken);
            _canvas.alpha = 1f;
            while (duration > 0)
            {
                SetTexts(duration);
                await UniTask.Delay(1_000, false, PlayerLoopTiming.Update, unifiedToken);
                duration--;
            }
            await FadeOutAsync(MatchStartFadeDuration, unifiedToken);

            void SetTexts(int s)
            {
                _leftText.text = string.Format(MatchStartText, s);
                _rightText.text = string.Format(MatchStartText, s);
            }
        }
        
        /// <summary>
        /// Announces that a goal has been scored asynchronously.
        /// </summary>
        /// <param name="player">The <see cref="Player"/> that scored the goal.</param>
        /// <param name="duration">Teh duration of the announcement, in seconds.</param>
        /// <param name="token">The token for operation cancellation.</param>
        /// <returns>The awaitable task.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="duration"/>
        /// is negative.</exception>
        /// <exception cref="NotImplementedException">Thrown if an invalid <see cref="Player"/>
        /// was provided.</exception>
        internal async UniTask AnnounceGoalAsync(Player player, int duration, CancellationToken token)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration must be positive.");
            
            switch (player)
            {
                case Player.LeftPlayer:
                    _leftText.text = ScoredText;
                    _rightText.text = string.Format(OtherScoreText, 1);
                    break;
                case Player.RightPlayer:
                    _leftText.text = string.Format(OtherScoreText, 2);
                    _rightText.text = ScoredText;
                    break;
                default:
                    throw new NotImplementedException($"Player not valid: {player}");
            }
            
            var unifiedToken = token.Unify(_cancellationTokenSource.Token);
            await FadeInAsync(duration * 0.1f, unifiedToken);
            await UniTask.Delay((int) (duration * 1_000 * 0.8f), false, PlayerLoopTiming.Update, unifiedToken);
            await FadeOutAsync(duration * 0.1f, unifiedToken);
        }

        /// <summary>
        /// Displays a "get ready" announcement to the players asynchronously.
        /// </summary>
        /// <param name="duration">The duration of the announcement, in seconds.</param>
        /// <param name="token">The token for operation cancellation.</param>
        /// <returns>The awaitable task.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="duration"/>
        /// is negative.</exception>
        internal async UniTask AnnounceGetReadyAsync(int duration, CancellationToken token)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration must be positive.");
            
            _leftText.text = GetReadyText;
            _rightText.text = GetReadyText;
            var unifiedToken = token.Unify(_cancellationTokenSource.Token);
            await FadeInAsync(duration * 0.1f, unifiedToken);
            await UniTask.Delay((int) (duration * 1_000 * 0.9f), false, PlayerLoopTiming.Update, unifiedToken);
            _leftText.text = GoText;
            _rightText.text = GoText;
        }

        /// <summary>
        /// Fades the board out, regardless of what's been shown.
        /// </summary>
        /// <param name="token">The token for operation cancellation.</param>
        /// <returns>The awaitable task.</returns>
        internal async UniTask FadeOutAsync(CancellationToken token)
        {
	        var unifiedToken = token.Unify(_cancellationTokenSource.Token);
            await FadeOutAsync(FadeOutDuration, unifiedToken);
        }

        internal async UniTask AnnounceEndOfMatchAsync(MatchResult matchResult, CancellationToken token)
        {
            switch (matchResult)
            {
                case MatchResult.Tie:
                    _leftText.text = TieText;
                    _rightText.text = TieText;
                    break;
                case MatchResult.LeftPlayerWin:
                    _leftText.text = YouWinText;
                    _rightText.text = YouLoseText;
                    break;
                case MatchResult.RightPlayerWin:
                    _leftText.text = YouLoseText;
                    _rightText.text = YouWinText;
                    break;
                default:
                    throw new NotImplementedException($"Result not valid: {matchResult}");
            }

            await FadeInAsync(MatchEndFadeDuration, token);
        }

        #endregion

        #region Private

        private void SetAlpha(float alpha) => _canvas.alpha = alpha;
        
        /// <summary>
        /// Fades the announcement board out asynchronously. 
        /// </summary>
        /// <param name="duration">The duration of the fade, in seconds.</param>
        /// <param name="token">The token for operation cancellation.</param>
        /// <returns>The awaitable task.</returns>
        private async UniTask FadeOutAsync(float duration, CancellationToken token)
        {
	        var unifiedToken = token.Unify(_cancellationTokenSource.Token);
            await UniTaskExtensions.ProgressAsync(SetAlpha, 1f, 0f, duration, unifiedToken);
        }
        
        /// <summary>
        /// Fades the announcement board in asynchronously. 
        /// </summary>
        /// <param name="duration">The duration of the fade, in seconds.</param>
        /// <param name="token">The token for operation cancellation.</param>
        /// <returns>The awaitable task.</returns>
        private async UniTask FadeInAsync(float duration, CancellationToken token)
        {
	        var unifiedToken = token.Unify(_cancellationTokenSource.Token);
            await UniTaskExtensions.ProgressAsync(SetAlpha, 0f, 1f, duration, unifiedToken);
        }

        #endregion
    }
}