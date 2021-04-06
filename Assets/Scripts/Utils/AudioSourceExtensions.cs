using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AirHockey.Utils
{
    public static class AudioSourceExtensions
    {
        #region Public

        /// <summary>
        /// Fades an <see cref="AudioSource"/> in asynchronously. 
        /// </summary>
        /// <param name="source">The <see cref="AudioSource"/> to be faded.</param>
        /// <param name="end">The volume end value.</param>
        /// <param name="duration">The duration of the fade in seconds.</param>
        /// <param name="token">The token for operation cancellation.</param>
        /// <returns>The awaitable task.</returns>
        public static async UniTask FadeInAsync(this AudioSource source, float end, float duration, CancellationToken token)
        {
            await UniTaskExtensions.ProgressAsync(SetVolume, 0f, end, duration, token);

            void SetVolume(float volume) => source.volume = volume;
        }
        
        /// <summary>
        /// Fades an <see cref="AudioSource"/> out asynchronously. 
        /// </summary>
        /// <param name="source">The <see cref="AudioSource"/> to be faded.</param>
        /// <param name="duration">The duration of the fade in seconds.</param>
        /// <param name="token">The token for operation cancellation.</param>
        /// <returns>The awaitable task.</returns>
        public static async UniTask FadeOutAsync(this AudioSource source,float duration, CancellationToken token)
        {
            await UniTaskExtensions.ProgressAsync(SetVolume, source.volume, 0f, duration, token);

            void SetVolume(float volume) => source.volume = volume;
        }

        #endregion
    }
}