using System;
using AirHockey.Match.Managers;
using UniRx.Async;

namespace AirHockey.Match.Referees
{
    public class EndlessReferee : Referee
    {
        #region Setup

        public EndlessReferee(Action pause, Resumer resume, Action end, ScoreManager manager) 
            : base(pause, resume, end, manager)
        {
        }

        #endregion

        #region Event handlers

        protected override void HandleScore(Player player, Score score)
        {
            Pause();
            Resume(player).Forget();
        }

        #endregion
    }
}