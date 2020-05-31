using System;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Match
{
    public class AnnouncementBoard : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private CanvasGroup _canvas;
        [SerializeField] private Text _text;

        #endregion

        #region Fields

        private const string MatchStartText = "The match starts in {0}...";
        private const string ScoreText = "Player {0} scored!";
        private const string GetReadyText = "On your marks...";
        private const string GoText = "GO!";

        #endregion

        #region Public

        public async UniTask AnnounceMatchStartAsync(int seconds)
        {
            FadeIn(0.5f);
            _canvas.alpha = 1f;
            while (seconds > 0)
            {
                _text.text = String.Format(MatchStartText, seconds);
                await UniTask.Delay(1_000);
                seconds--;
            }
        }

        public async Task AnnouncePlayerScored(int playerID, int seconds)
        {
            FadeIn(0.5f);
            _text.text = String.Format(ScoreText, playerID);
            await UniTask.Delay(seconds * 1_000);
        }

        public async Task AnnounceGetReadyAsync(int seconds)
        {
            _text.text = GetReadyText;
            await UniTask.Delay(seconds * 1_000);
            _text.text = GoText;
            await FadeOut(1f);
        }

        #endregion

        #region Private

        private async Task FadeOut(float duration)
        {
            var totalTime = 0f;
            while (totalTime <= duration)
            {
                await UniTask.Yield();
                _canvas.alpha = 1 - totalTime / duration;
                totalTime += Time.deltaTime;
            }
        }
        
        private async void FadeIn(float duration)
        {
            var totalTime = 0f;
            while (totalTime <= duration)
            {
                await UniTask.Yield();
                _canvas.alpha = totalTime / duration;
                totalTime += Time.deltaTime;
            }
        }

        #endregion
    }
}