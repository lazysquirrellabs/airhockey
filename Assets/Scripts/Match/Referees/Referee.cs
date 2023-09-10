using System;
using System.Threading;
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
        internal delegate UniTask AsyncPauser(Player p, CancellationToken token);

        #endregion
        
        #region Properties    

        /// <summary>
        /// Pauses the match when a player scores.
        /// </summary>
        protected AsyncPauser PauseAsync { get; }
        
        /// <summary>
        /// Ends the match.
        /// </summary>
        protected Func<UniTask> EndAsync { get; }

        #endregion

        #region Setup

        /// <summary>
        /// <see cref="Referee"/>'s constructor
        /// </summary>
        /// <param name="pause">How to pause the game whenever a player scores.</param>
        /// <param name="endAsync">How to end the game.</param>
        protected Referee(AsyncPauser pause, Func<UniTask> endAsync)
        {
            PauseAsync = pause;
            EndAsync = endAsync;
        }

        #endregion

        #region Internal

        /// <summary>
        /// Handles the event of a player score.
        /// </summary>
        /// <param name="player">The player who scored.</param>
        /// <param name="score">The updated match score.</param>
        /// <param name="token">Token used for task cancellation.</param>
        internal abstract UniTask ProcessScoreAsync(Player player, Score score, CancellationToken token);
        
        /// <summary>
        /// Removes the referee from the match.
        /// </summary>
        internal virtual void LeaveMatch()
        {
        }

        #endregion
    }
}