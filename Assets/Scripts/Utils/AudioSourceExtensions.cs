using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AirHockey.Utils
{
    public static class AudioSourceExtensions
    {
        #region Public

        public static async UniTask FadeIn(this AudioSource source, float end, float duration, CancellationToken token)
        {
            await UniTaskExtensions.ProgressAsync(SetVolume, 0f, end, duration, token);

            void SetVolume(float volume) => source.volume = volume;

        }
        
        public static async UniTask FadeOut(this AudioSource source,float duration, CancellationToken token)
        {
            await UniTaskExtensions.ProgressAsync(SetVolume, source.volume, 0f, duration, token);

            void SetVolume(float volume) => source.volume = volume;
        }

        #endregion
    }
}