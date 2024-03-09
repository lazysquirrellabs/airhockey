using System.Threading;
using LazySquirrelLabs.AirHockey.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniTaskExtensions = LazySquirrelLabs.AirHockey.Utils.UniTaskExtensions;

namespace LazySquirrelLabs.AirHockey.UI
{
    /// <summary>
    /// Fades a <see cref="CanvasGroup"/> in out asynchronously.
    /// </summary>
    internal class CanvasFader : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;

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
        /// Fades the <see cref="Canvas"/> in, asynchronously.
        /// </summary>
        /// <param name="duration">The duration of the fade, in seconds.</param>
        /// <param name="token">Token used for task cancellation.</param>
        /// <returns>The awaitable task.</returns>
        internal async UniTask FadeInAsync(float duration, CancellationToken token)
        {
	        var unifiedToken = token.Unify(_cancellationTokenSource.Token);
	        _canvas.enabled = true;
            await FadeAsync(0f, 1f, duration, unifiedToken);
        }
        
        /// <summary>
        /// Fades the <see cref="Canvas"/> out, asynchronously.
        /// </summary>
        /// <param name="duration">The duration of the fade, in seconds.</param>
        /// <param name="token">Token used for task cancellation.</param>
        /// <returns>The awaitable task.</returns>
        internal async UniTask FadeOutAsync(float duration, CancellationToken token)
        {
	        var unifiedToken = token.Unify(_cancellationTokenSource.Token);
            await FadeAsync(1f, 0f, duration, unifiedToken);
            _canvas.enabled = false;
        }

        #endregion

        #region Private

        /// <summary>
        /// Fades a <see cref="Canvas"/> asynchronously.
        /// </summary>
        /// <param name="from">The start value of the <see cref="Canvas"/>'s alpha.</param>
        /// <param name="to">The end value of the <see cref="Canvas"/>'s alpha.</param>
        /// <param name="duration">The duration of the fade, in seconds.</param>
        /// <param name="token">Token used for task cancellation.</param>
        /// <returns>The awaitable task.</returns>
        private async UniTask FadeAsync(float from, float to, float duration, CancellationToken token)
        {
            await UniTaskExtensions.ProgressAsync(SetAlpha, from, to, duration, token);
            
            void SetAlpha(float alpha) => _canvasGroup.alpha = alpha;
        }

        #endregion
    }
}