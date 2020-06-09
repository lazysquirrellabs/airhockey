using System;
using AirHockey.Match.Managers;
using UniRx.Async;

namespace AirHockey.Match.Referees
{
    public abstract class Referee
    {
        #region Delegates

        public delegate UniTask Resumer(Player p);

        #endregion
        
        #region Properties    

        protected Action Pause { get; }
        protected Resumer Resume { get; }
        protected Action End { get; }

        #endregion

        #region Fields

        private readonly Action _stopListening;

        #endregion

        #region Setup

        protected Referee(Action pause, Resumer resume, Action end, ScoreManager manager)
        {
            Pause = pause;
            Resume = resume;
            End = end;
            
            _stopListening = () => manager.OnScore -= HandleScore;
            manager.OnScore += HandleScore;
        }

        #endregion

        #region Event handlers

        protected abstract void HandleScore(Player player, Score score);

        #endregion

        #region Public

        public virtual void CancelMatch()
        {
            _stopListening?.Invoke();
        }

        #endregion
    }
}