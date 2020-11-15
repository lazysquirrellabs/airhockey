using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Match
{
    public class AnnouncementBoard : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private CanvasGroup _canvas;
        [SerializeField] private Text _leftText;
        [SerializeField] private Text _rightText;

        #endregion

        #region Fields

        private const string MatchStartText = "The match starts in {0}...";
        private const string ScoredText = "Goal!!!";
        private const string OtherScoreText = "Player {0} scored";
        private const string GetReadyText = "On your marks...";
        private const string GoText = "GO!";
        private const string YouWinText = "You win!!!";
        private const string YouLoseText = "You lose.";
        private const string TieText = "It's a tie.";

        #endregion

        #region Public

        public async UniTask AnnounceMatchStartAsync(int seconds, CancellationToken token)
        {
            if (seconds < 0)
                throw new ArgumentOutOfRangeException(nameof(seconds), seconds, "Duration must be positive.");
            
            SetTexts(seconds);
            await FadeInAsync(500f, token);
            _canvas.alpha = 1f;
            while (seconds > 0)
            {
                SetTexts(seconds);
                await UniTask.Delay(1_000, false, PlayerLoopTiming.Update, token);
                seconds--;
            }
            await FadeOutAsync(500f, token);

            void SetTexts(int s)
            {
                _leftText.text = string.Format(MatchStartText, s);
                _rightText.text = string.Format(MatchStartText, s);
            }
        }
        
        public async UniTask AnnounceGoalAsync(Player player, int duration, CancellationToken token)
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
            
            await FadeInAsync(duration * 0.1f, token);
            await UniTask.Delay((int) (duration * 0.8f), false, PlayerLoopTiming.Update, token);
            await FadeOutAsync(duration * 0.1f, token);
        }

        public async UniTask AnnounceGetReadyAsync(int duration, CancellationToken token)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration must be positive.");
            
            _leftText.text = GetReadyText;
            _rightText.text = GetReadyText;
            await FadeInAsync(duration * 0.1f, token);
            await UniTask.Delay((int) (duration * 0.9f), false, PlayerLoopTiming.Update, token);
            _leftText.text = GoText;
            _rightText.text = GoText;
            FadeOutAsync(1_000, token).Forget();
        }

        public void AnnounceEndOfMatch(Score.Result result, CancellationToken token)
        {
            switch (result)
            {
                case Score.Result.Tie:
                    _leftText.text = TieText;
                    _rightText.text = TieText;
                    break;
                case Score.Result.LeftPlayerWin:
                    _leftText.text = YouWinText;
                    _rightText.text = YouLoseText;
                    break;
                case Score.Result.RightPlayerWin:
                    _leftText.text = YouLoseText;
                    _rightText.text = YouWinText;
                    break;
                default:
                    throw new NotImplementedException($"Result not valid: {result}");
            }
            
            FadeInAsync(0.5f, token).Forget();
        }

        #endregion

        #region Private
        
        private async UniTask FadeOutAsync(float duration, CancellationToken token)
        {
            var totalTime = 0f;
            while (totalTime <= duration)
            {
                _canvas.alpha = 1 - totalTime / duration;
                totalTime += Time.deltaTime * 1_000;
                await UniTask.Yield();
                token.ThrowIfCancellationRequested();
            }

            _canvas.alpha = 0f;
        }
        
        private async UniTask FadeInAsync(float duration, CancellationToken token)
        {
            var totalTime = 0f;
            while (totalTime <= duration)
            {
                _canvas.alpha = totalTime / duration;
                totalTime += Time.deltaTime * 1_000;
                await UniTask.Yield();
                token.ThrowIfCancellationRequested();
            }

            _canvas.alpha = 1f;
        }

        #endregion
    }
}