using System;
using Cysharp.Threading.Tasks;

namespace AirHockey.Match.Referees
{
    public abstract class Referee
    {
        #region Delegates
        public delegate UniTask Pauser(Player p);

        #endregion
        
        #region Properties    

        protected Pauser Pause { get; }
        protected Action End { get; }

        #endregion

        #region Setup

        protected Referee(Pauser pause, Action end, Action<Scorer> subscribeToScore)
        {
            Pause = pause;
            End = end;
            subscribeToScore(HandleScore);
        }

        #endregion

        #region Event handlers

        protected abstract void HandleScore(Player player, Score score);

        #endregion

        #region Public

        public virtual void CancelMatch(Action<Scorer> unsubscribeToScore)
        {
            unsubscribeToScore(HandleScore);
        }

        #endregion
    }
}