using System;
using UniRx.Async;
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

        #endregion

        #region Public

        public async UniTask AnnounceMatchStartAsync(int seconds)
        {
            SetTexts(seconds);
            await FadeInAsync(500f);
            _canvas.alpha = 1f;
            while (seconds > 0)
            {
                SetTexts(seconds);
                await UniTask.Delay(1_000);
                seconds--;
            }
            await FadeOutAsync(500f);

            void SetTexts(int s)
            {
                _leftText.text = String.Format(MatchStartText, s);
                _rightText.text = String.Format(MatchStartText, s);
            }
        }

        public async UniTask AnnouncePlayerScoredAsync(Player player, int duration)
        {
            switch (player)
            {
                case Player.LeftPlayer:
                    _leftText.text = ScoredText;
                    _rightText.text = String.Format(OtherScoreText, 1);
                    break;
                case Player.RightPlayer:
                    _leftText.text = String.Format(OtherScoreText, 2);
                    _rightText.text = ScoredText;
                    break;
                default:
                    throw new NotImplementedException($"Player not valid: {player}");
            }
            
            await FadeInAsync(duration * 0.1f);
            await UniTask.Delay((int) (duration * 0.8f));
            await FadeOutAsync(duration * 0.1f);
        }

        public async UniTask AnnounceGetReadyAsync(int duration)
        {
            _leftText.text = GetReadyText;
            _rightText.text = GetReadyText;
            await FadeInAsync(duration * 0.1f);
            await UniTask.Delay((int) (duration * 0.9f));
            _leftText.text = GoText;
            _rightText.text = GoText;
            FadeOutAsync(1_000).Forget();
        }

        #endregion

        #region Private
        private async UniTask FadeOutAsync(float duration)
        {
            var totalTime = 0f;
            while (totalTime <= duration)
            {
                _canvas.alpha = 1 - totalTime / duration;
                totalTime += Time.deltaTime * 1_000;
                await UniTask.Yield();
            }

            _canvas.alpha = 0f;
        }
        
        private async UniTask FadeInAsync(float duration)
        {
            var totalTime = 0f;
            while (totalTime <= duration)
            {
                _canvas.alpha = totalTime / duration;
                totalTime += Time.deltaTime * 1_000;
                await UniTask.Yield();
            }

            _canvas.alpha = 1f;
        }

        #endregion
    }
}