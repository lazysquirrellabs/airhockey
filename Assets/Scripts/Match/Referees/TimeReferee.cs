using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AirHockey.Match.Referees
{
    public class TimeReferee : Referee
    {
        #region Fields

        private bool _running;
        private readonly CancellationTokenSource _tokenSource;
        private readonly Action<uint> _onUpdate;

        #endregion
        
        #region Setup

        public TimeReferee(Pauser pause, Action end, Action<Scorer> subscribe, uint min, Action<uint> onUpdate) 
            : base(pause, end, subscribe)
        {
            _tokenSource = new CancellationTokenSource();
            _running = true;
            _onUpdate = onUpdate;
            StartTimer(min * 60, _tokenSource.Token).Forget();
        }
        
        #endregion

        #region Event handlers

        protected override async void HandleScore(Player player, Score _)
        {
            _running = false;
            try
            {
                await Pause(player);
                _running = true;

            }
            catch (OperationCanceledException)
            {
                Debug.Log("Timed referee stopped because the operation was cancelled.");
            }
        }

        #endregion

        #region Public

        public override void CancelMatch(Action<Scorer> unsubscribeToScore)
        {
            base.CancelMatch(unsubscribeToScore);
            Stop();
        }

        #endregion

        #region Private

        /// <summary>
        /// Starts a timer with the provided amount of  <paramref name="seconds"/> in it. This async method should not
        /// be awaited, just fire it and forget.
        /// </summary>
        /// <param name="seconds">The duration of the timer.</param>
        /// <param name="token">The cancellation token used to stop the timer.</param>
        /// <returns>The required <see cref="UniTaskVoid"/> to forget the task.</returns>
        private async UniTaskVoid StartTimer(uint seconds, CancellationToken token)
        {
            const int ticksPerSecond = 10;
            const int tickInterval = 1_000 / ticksPerSecond;
            var milliseconds = seconds * 1_000;
            // Counter that keeps track of how many times the timer ticked this second.
            var ticks = 0;
            // Elapsed time in milliseconds since the timer started.
            uint elapsed = 0;
            while (!token.IsCancellationRequested)
            {
                await TryToTick();

                // If the timer is paused, don't tick.
                if (!_running) continue;
                
                elapsed += tickInterval;
                ticks++;
                
                // If the number of ticks within this second is enough,
                // invoke the onUpdate and restart the counter.
                if (ticks >= ticksPerSecond)
                {
                    _onUpdate?.Invoke(seconds - elapsed / 1_000);
                    ticks = 0;
                }

                // If the timer ran to completion, stop it, call the end delegate and return.
                if (elapsed >= milliseconds)
                {
                    Stop();
                    End();
                    return;
                }
            }

            bool IsRunning() => _running;
            
            async UniTask TryToTick()
            {
                // If the token is cancelled, an OperationCanceledException is thrown.
                try
                {
                    var waitRunning = UniTask.WaitUntil(IsRunning, PlayerLoopTiming.Update, token);
                    var waitDelay = UniTask.Delay(tickInterval, false, PlayerLoopTiming.Update, token);
                    await UniTask.WhenAll(waitRunning, waitDelay);
                }
                catch (OperationCanceledException)
                {
                    Debug.Log("The timer stopped because the token was cancelled.");
                }
            }
        }
        
        private void Stop()
        {
            _running = false;
            _tokenSource.Cancel();
        }

        #endregion
    }
}