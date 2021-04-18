using System;
using UnityEngine;

namespace AirHockey.Match.Referees
{
    /// <summary>
    /// <see cref="Referee"/> which never ends the match and lets the players play endlessly.
    /// </summary>
    public class 
    EndlessReferee : Referee
    {
        #region Setup

        /// <summary>
        /// <see cref="EndlessReferee"/>'s constructor.
        /// </summary>
        /// <param name="pause">How to pause the match when a player scores.</param>
        /// <param name="end">How to end the match. Although this <see cref="Referee"/> never ends the match
        /// automatically, it can be done via player input. </param>
        /// <param name="subscribeToScore">How to subscribe to the match scoring.</param>
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