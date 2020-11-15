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
        [SerializeField, Range(0,10)] private float _duration;

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
            await UniTaskExtensions.ProgressAsync(SetAlpha, from, to, _duration, _cancellationTokenSource.Token);
            
            void SetAlpha(float alpha) => _canvasGroup.alpha = alpha;
        }

        #endregion
    }
}