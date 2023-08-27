using System.Threading;
using AirHockey.Utils;
using UnityEngine;
using UniTask = Cysharp.Threading.Tasks.UniTask;

namespace AirHockey.Match.Managers
{
    /// <summary>
    /// A match's audio manager.
    /// </summary>
    internal class AudioManager : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private AudioSource _loop;
        [SerializeField] private AudioSource _goalCrowd;
        [SerializeField] private AudioSource _goalHorn;
        [SerializeField] private AudioSource _buzz;

        #endregion

        #region Fields

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        #endregion

        #region Setup

        private async void Awake()
        {
            await _loop.FadeInAsync(1f, 3f, _cancellationTokenSource.Token);
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        #endregion

        #region Internal

        /// <summary>
        /// Fades all match audio out.
        /// </summary>
        /// <param name="duration">The duration of the fade in seconds.</param>
        /// <returns>The awaitable task.</returns>
        internal async UniTask FadeOutAllAsync(float duration)
        {
            var goalHorn = _goalHorn.FadeOutAsync(duration, _cancellationTokenSource.Token);
            var goalCrowd = _goalCrowd.FadeOutAsync(duration, _cancellationTokenSource.Token);
            var loop = _loop.FadeOutAsync(duration, _cancellationTokenSource.Token);
            await UniTask.WhenAll(goalHorn, goalCrowd, loop);
        }

        /// <summary>
        /// Plays the sound effects for when a player scores. 
        /// </summary>
        internal void PlayGoal()
        {
            _goalCrowd.Play();
            _goalHorn.Play();
        }

        /// <summary>
        /// Plays a buzz sound.
        /// </summary>
        internal void PlayBuzz()
        {
            _buzz.Play();
        }

        #endregion
    }
}