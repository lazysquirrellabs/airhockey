using System;
using UnityEngine;

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

        protected override async void HandleScore(Player player, Score score)
        {
            try
            {
                await PauseAsync(player);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"{typeof(EndlessReferee)} failed to handle score because the operation was cancelled.");
            }
        }

        #endregion
    }
}