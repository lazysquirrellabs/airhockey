using System;
using System.Threading;
using AirHockey.Match.Scoring;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AirHockey.Match.Referees
{
    /// <summary>
    /// A <see cref="Referee"/> which controls the <see cref="Match"/> lifetime based on a timer.
    /// </summary>
    internal class TimeReferee : Referee
    {
        #region Fields

        private bool _running;
        private readonly CancellationTokenSource _tokenSource;
        private readonly Action<uint> _onUpdate;
        private readonly uint _duration;

        #endregion
        
        #region Setup

        /// <summary>
        /// <see cref="TimeReferee"/>'s constructor.
        /// </summary>
        /// <param name="pause">How to pause the game whenever a player scores.</param>
        /// <param name="end">How to end the game.</param>
        /// <param name="duration">The desired duration of the match in seconds.</param>
        /// <param name="onUpdate">Callback to be invoked every time the timer ticks.</param>
        internal TimeReferee(AsyncPauser pause, Action end, uint duration, Action<uint> onUpdate) : base(pause, end)
        {
            _tokenSource = new CancellationTokenSource();
            _running = true;
            _onUpdate = onUpdate;
            _duration = duration;
        }
        
        #endregion

        #region Internal

        internal override async UniTask ProcessScoreAsync(Player player, Score _, CancellationToken token)
        {
	        _running = false;
	        await PauseAsync(player, token);
        }
        
        internal override void LeaveMatch()
        {
            Stop();
        }
        
        /// <summary>
        /// Starts a timer. This async method should not be awaited, just fire it and forget.
        /// </summary>
        /// <returns>The required <see cref="UniTaskVoid"/> to forget the task.</returns>
        internal async UniTaskVoid StartTimer()
        {
            var token = _tokenSource.Token;
            const int ticksPerSecond = 10;
            const int tickInterval = 1_000 / ticksPerSecond;
            var durationMilli = _duration * 1_000;
            // Counter that keeps track of how many times the timer ticked this second.
            var ticks = 0;
            // Elapsed time in milliseconds since the timer started.
            uint elapsed = 0;
            try
            {
                while (true)
                {
                    var waitRunning = UniTask.WaitUntil(IsRunning, PlayerLoopTiming.Update, token);
                    var waitDelay = UniTask.Delay(tickInterval, false, PlayerLoopTiming.Update, token);
                    await UniTask.WhenAll(waitRunning, waitDelay);

                    // If the timer is paused, don't tick.
                    if (!_running)
                        continue;

                    elapsed += tickInterval;
                    ticks++;

                    // If the number of ticks within this second is enough,
                    // invoke the onUpdate and restart the counter.
                    if (ticks >= ticksPerSecond)
                    {
                        _onUpdate?.Invoke(_duration - elapsed / 1_000);
                        ticks = 0;
                    }

                    // If the timer ran to completion, stop it, call the end delegate and return.
                    if (elapsed >= durationMilli)
                    {
                        Stop();
                        End();
                        return;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"The {typeof(TimeReferee)}'s timer stopped because the operation was cancelled.");
            }

            bool IsRunning() => _running;
        }

        #endregion

        #region Private
        
        /// <summary>
        /// Stops the match timer.
        /// </summary>
        private void Stop()
        {
            _running = false;
            _tokenSource.Cancel();
        }

        #endregion
    }
}