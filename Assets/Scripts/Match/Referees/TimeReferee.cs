using System;
using System.Timers;
using AirHockey.Match.Managers;

namespace AirHockey.Match.Referees
{
    public class TimeReferee : Referee
    {
        #region Fields

        private bool _running;
        private uint _elapsed;
        private readonly Timer _timer;
        private readonly uint _duration;
        private readonly Action _stopListening;

        #endregion
        
        #region Setup

        public TimeReferee(Action pause, Resumer resume, Action end, ScoreManager manager, uint duration) 
            : base(pause, resume, end)
        {
            _duration = duration * 1_000;
            manager.OnScore += HandleScoreAsync;
            _stopListening = () => manager.OnScore -= HandleScoreAsync;
            _timer = new Timer(1000);
            _timer.Elapsed += HandleTimerElapsed;
            _timer.Start();
            _running = true;
        }
        
        #endregion

        #region Event handlers

        private void HandleTimerElapsed(object source, ElapsedEventArgs e)
        {
            if (!_running) return;
            
            _elapsed += 1_000;

            if (_elapsed >= _duration)
            {
                Stop();
                End();
            }
        }

        private async void HandleScoreAsync(Player player, Score _)
        {
            _timer.Stop();
            _running = false;
            Pause();
            await Resume(player);
            _running = true;
            _timer.Start();
        }

        #endregion

        #region Public

        public override void CancelMatch()
        {
            Stop();
        }

        #endregion

        #region Private

        private void Stop()
        {
            _running = false;
            _timer.Stop();
            _timer.Elapsed -= HandleTimerElapsed;
            _stopListening?.Invoke();
        }

        #endregion
    }
}