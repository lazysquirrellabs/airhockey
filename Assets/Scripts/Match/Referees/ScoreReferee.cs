using System;
using System.Threading;
using AirHockey.Match.Scoring;
using Cysharp.Threading.Tasks;

namespace AirHockey.Match.Referees
{
    /// <summary>
    /// A <see cref="Referee"/> which controls the <see cref="Match"/> lifetime based on the <see cref="Score"/>.
    /// </summary>
    internal abstract class ScoreReferee : Referee
    {
        #region Delegates
        
        /// <summary>
        /// Encapsulates a method which analyzes the given <see cref="Score"/> and returns whether the match should end.
        /// </summary>
        protected delegate bool ScoreCheck(Score score);

        #endregion   
        
        #region Fields

        /// <summary>
        /// Checks whether a given score should end the match.
        /// </summary>
        private readonly ScoreCheck _isOver;

        #endregion

        #region Setup
        
        /// <summary>
        /// <see cref="ScoreReferee"/>'s constructor.
        /// </summary>
        /// <param name="pause">How to pause the game whenever a player scores.</param>
        /// <param name="endAsync">How to end the game.</param>
        /// <param name="isOver">How to check if the score should end the match.</param>
        protected ScoreReferee(AsyncPauser pause, Func<UniTask> endAsync, ScoreCheck isOver) : base(pause, endAsync)
        {
            _isOver = isOver;
        }
        
        #endregion
        
        #region Internal

        internal override async UniTask ProcessScoreAsync(Player player, Score score, CancellationToken token)
        {
	        if (_isOver(score))
	        {
		        await EndAsync();
	        }
	        else
            {
                await PauseAsync(player, token);
            }
        }

        #endregion
    }
}