using System;
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

        #region Setup

        protected Referee(Action pause, Resumer resume, Action end)
        {
            Pause = pause;
            Resume = resume;
            End = end;
        }

        #endregion

        #region Public

        public virtual void StartMatch() {}
        
        public abstract void CancelMatch();

        #endregion
    }
}