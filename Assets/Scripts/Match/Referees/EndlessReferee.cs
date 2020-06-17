using System;
using UniRx.Async;

namespace AirHockey.Match.Referees
{
    public class EndlessReferee : Referee
    {
        #region Setup

        public EndlessReferee(Pauser pause, Action end, Action<Scorer> subscribeToScore) 
            : base(pause, end, subscribeToScore)
        {
        }

        #endregion

        #region Event handlers

        protected override void HandleScore(Player player, Score score)
        {
            Pause(player).Forget();
        }

        #endregion
    }
}