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

        protected Pauser PauseAsync { get; }
        protected Action End { get; }

        #endregion

        #region Setup

        protected Referee(Pauser pause, Action end, Action<Scorer> subscribeToScore)
        {
            PauseAsync = pause;
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