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

        /// <summary>
        /// Fades the <see cref="Canvas"/> in, asynchronously.
        /// </summary>
        /// <param name="duration">The duration of the fade, in seconds.</param>
        /// <returns>The awaitable task.</returns>
        public async UniTask FadeInAsync(float duration)
        {
            await FadeAsync(0f, 1f, duration);
        }
        
        /// <summary>
        /// Fades the <see cref="Canvas"/> out, asynchronously.
        /// </summary>
        /// <param name="duration">The duration of the fade, in seconds.</param>
        /// <returns>The awaitable task.</returns>
        public async UniTask FadeOutAsync(float duration)
        {
            await FadeAsync(1f, 0f, duration);
        }

        #endregion

        #region Private

        /// <summary>
        /// Fades a <see cref="Canvas"/> asynchronously.
        /// </summary>
        /// <param name="from">The start value of the <see cref="Canvas"/>'s alpha.</param>
        /// <param name="to">The end value of the <see cref="Canvas"/>'s alpha.</param>
        /// <param name="duration">The duration of the fade, in seconds.</param>
        /// <returns>The awaitable task.</returns>
        private async UniTask FadeAsync(float from, float to, float duration)
        {
            await UniTaskExtensions.ProgressAsync(SetAlpha, from, to, duration, _cancellationTokenSource.Token);
            
            void SetAlpha(float alpha) => _canvasGroup.alpha = alpha;
        }

        #endregion
    }
}