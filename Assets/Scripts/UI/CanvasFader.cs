using UniRx.Async;
using UnityEngine;

namespace AirHockey.UI
{
    public class CanvasFader : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Range(0,10)] private float _duration;

        #endregion

        #region Public

        public async UniTask FadeInAsync()
        {
            await FadeAsync(0f, 1f);
        }
        
        public async UniTask FadeOutAsync()
        {
            await FadeAsync(1f, 0f);
        }

        #endregion

        #region Private

        private async UniTask FadeAsync(float from, float to)
        {
            var startTime = Time.time;
            var delta = 0f;
            
            while (delta <= _duration)
            {
                await UniTask.Yield();
                _canvasGroup.alpha = Mathf.Lerp(from, to, delta / _duration);
                delta = Time.time - startTime;
            }

            _canvasGroup.alpha = to;
        }

        #endregion
    }
}