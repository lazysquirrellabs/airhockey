using System;
using AirHockey.Match.Scoring;
using Cysharp.Threading.Tasks;

namespace AirHockey.Match.Referees
{
    /// <summary>
    /// Base class for all match <see cref="Referee"/>s. A <see cref="Referee"/> is responsible for implementing the
    /// rules that reflect a match <see cref="MatchMode"/>. It has some capabilities like pausing and ending the match.
    /// </summary>
    internal abstract class Referee
    {
        #region Delegates
        
        /// <summary>
        /// Encapsulates an asynchronous method which can be used to pause the match whenever the given player scores.
        /// </summary>
        internal delegate UniTask Pauser(Player p);

        #endregion
        
        #region Properties    

        /// <summary>
        /// Pauses the match when a player scores.
        /// </summary>
        protected Pauser PauseAsync { get; }
        /// <summary>
        /// Ends the match.
        /// </summary>
        protected Action End { get; }

        #endregion

        #region Setup

        /// <summary>
        /// <see cref="Referee"/>'s constructor
        /// </summary>
        /// <param name="pause">How to pause the game whenever a player scores.</param>
        /// <param name="end">How to end the game.</param>
        /// <param name="subscribeToScore">How to subscribe to the match scoring.</param>
        protected Referee(Pauser pause, Action end, Action<Scorer> subscribeToScore)
        {
            PauseAsync = pause;
            End = end;
            subscribeToScore(HandleScore);
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the event of a player score.
        /// </summary>
        /// <param name="player">The player who scored.</param>
        /// <param name="score">The updated match score.</param>
        protected abstract void HandleScore(Player player, Score score);

        #endregion

        #region Internal

        /// <summary>
        /// Removes the referee from the match.
        /// </summary>
        /// <param name="unsubscribeToScore">Delegate used to unsubscribe from the match scoring.</param>
        internal virtual void LeaveMatch(Action<Scorer> unsubscribeToScore)
        {
            unsubscribeToScore(HandleScore);
        }

        #endregion
    }
}