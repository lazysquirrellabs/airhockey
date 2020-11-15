using System.Threading;
using AirHockey.Utils;
using UnityEngine;
using UniTask = Cysharp.Threading.Tasks.UniTask;

namespace AirHockey.Match.Managers
{
    public class AudioManager : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private AudioSource _loop;

        #endregion

        #region Fields

        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Setup

        private async void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await UniTaskExtensions.ProgressAsync(SetVolume, 0f, 1f, 3f, _cancellationTokenSource.Token);
        }

        #endregion

        #region Public

        public async UniTask FadeOutAllAsync(float duration)
        {
            await UniTaskExtensions.ProgressAsync(SetVolume, 1f, 0f, duration, _cancellationTokenSource.Token);
        }

        #endregion

        #region Private

        private void SetVolume(float volume) => _loop.volume = volume;

        #endregion
    }
}