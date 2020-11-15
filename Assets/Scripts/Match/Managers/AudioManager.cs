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
        [SerializeField] private AudioSource _goalCrowd;
        [SerializeField] private AudioSource _goalHorn;
        [SerializeField] private AudioSource _buzz;

        #endregion

        #region Fields

        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Setup

        private async void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await _loop.FadeIn(1f, 3f, _cancellationTokenSource.Token);
        }

        #endregion

        #region Public

        public async UniTask FadeOutAllAsync(float duration)
        {
            var goalHorn = _goalHorn.FadeOut(duration, _cancellationTokenSource.Token);
            var goalCrowd = _goalCrowd.FadeOut(duration, _cancellationTokenSource.Token);
            var loop = _loop.FadeOut(duration, _cancellationTokenSource.Token);
            await UniTask.WhenAll(goalHorn, goalCrowd, loop);
        }

        public void PlayGoal()
        {
            _goalCrowd.Play();
            _goalHorn.Play();
        }

        public void PlayBuzz()
        {
            _buzz.Play();
        }

        #endregion
    }
}