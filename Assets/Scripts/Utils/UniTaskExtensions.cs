using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AirHockey.Utils
{
    public static class UniTaskExtensions
    {
        #region Public
        
        public static async UniTask ProgressAsync(Action<float> update, float start, float end, float duration, 
                                                  CancellationToken token)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration must be positive.");
            
            try
            {
                var startTime = Time.time;
                var delta = 0f;

                while (delta <= duration)
                {
                    var value = Mathf.Lerp(start, end, delta / duration);
                    update(value);
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                    token.ThrowIfCancellationRequested();
                    delta = Time.time - startTime;
                }

                update(end);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Stopped progressing because the operation got cancelled");
            }
        }

        #endregion
    }
}