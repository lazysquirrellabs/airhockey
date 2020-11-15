using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniTaskExtensions = AirHockey.Utils.UniTaskExtensions;

namespace AirHockey.UI
{
    public class CanvasFader : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private CanvasGroup _canvasGroup;

        #endregion

        #region Fields

        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Setup

        private void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
        }

        #endregion

        #region Public

        public async UniTask FadeInAsync(float duration)
        {
            await FadeAsync(0f, 1f, duration);
        }
        
        public async UniTask FadeOutAsync(float duration)
        {
            await FadeAsync(1f, 0f, duration);
        }

        #endregion

        #region Private

        private async UniTask FadeAsync(float from, float to, float duration)
        {
            await UniTaskExtensions.ProgressAsync(SetAlpha, from, to, duration, _cancellationTokenSource.Token);
            
            void SetAlpha(float alpha) => _canvasGroup.alpha = alpha;
        }

        #endregion
    }
}