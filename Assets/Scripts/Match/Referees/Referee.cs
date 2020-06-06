using System;

namespace AirHockey.Match.Referees
{
    public abstract class Referee
    {
        #region Properties    

        protected Action Pause { get; }
        protected Action<Player> Resume { get; }
        protected Action End { get; }

        #endregion

        #region Setup

        protected Referee(Action pause, Action<Player> resume, Action end)
        {
            Pause = pause;
            Resume = resume;
            End = end;
        }

        #endregion

        #region Public

        public abstract void CancelMatch();

        #endregion
    }
}