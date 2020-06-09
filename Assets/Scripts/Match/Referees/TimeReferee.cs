using System;
using System.Threading;
using AirHockey.Match.Managers;
using UniRx.Async;

namespace AirHockey.Match.Referees
{
    public class TimeReferee : Referee
    {
        #region Fields

        private bool _running;
        private uint _elapsed;
        private readonly CancellationTokenSource _tokenSource;
        private readonly Action<uint> _onTick;

        #endregion
        
        #region Setup

        public TimeReferee(Action pause, Resumer resume, Action end, ScoreManager manager, uint min, Action<uint> onTick) 
            : base(pause, resume, end, manager)
        {
            _tokenSource = new CancellationTokenSource();
            _running = true;
            _onTick = onTick;
            StartTimer(min * 60).Forget();
        }
        
        #endregion

        #region Event handlers

        protected override async void HandleScore(Player player, Score _)
        {
            _running = false;
            Pause();
            await Resume(player);
            _running = true;
        }

        #endregion

        #region Public

        public override void CancelMatch()
        {
            base.CancelMatch();
            Stop();
        }

        #endregion

        #region Private

        private async UniTaskVoid StartTimer(uint seconds)
        {
            var milliseconds = seconds * 1000;
            var token = _tokenSource.Token;
            while (true)
            {
                var waitRunning = UniTask.WaitUntil(IsRunning, PlayerLoopTiming.Update, token);
                var waitDelay =  UniTask.Delay(1_000, false, PlayerLoopTiming.Update, token);
                await UniTask.WhenAll(waitRunning, waitDelay);

                if (!_running) continue;
                
                _elapsed += 1_000;
                _onTick?.Invoke(seconds - _elapsed / 1000);

                if (_elapsed >= milliseconds)
                {
                    Stop();
                    End();
                    return;
                }
            }

            bool IsRunning() => _running;
        }
        
        private void Stop()
        {
            _running = false;
            _tokenSource.Cancel();
        }

        #endregion
    }
}