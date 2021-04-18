using System;
using UnityEngine;

namespace AirHockey.Match.Referees
{
    /// <summary>
    /// A <see cref="Referee"/> which controls the <see cref="Match"/> lifetime based on the <see cref="Score"/>.
    /// </summary>
    public abstract class ScoreReferee : Referee
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
        /// <param name="end">How to end the game.</param>
        /// <param name="isOver">How to check if the score should end the match.</param>
        /// <param name="subscribeToScore">How to subscribe to the match scoring.</param>
        protected ScoreReferee(Pauser pause, Action end, ScoreCheck isOver, Action<Scorer> subscribeToScore) 
            : base(pause, end, subscribeToScore)
        {
            _isOver = isOver;
        }
        
        #endregion
        
        #region Event hanlders

        protected override async void HandleScore(Player player, Score score)
        {
            if (_isOver(score)) 
                End();
            else
            {
                try
                {
                    await PauseAsync(player);
                }
                catch (OperationCanceledException)
                {
                    Debug.Log($"{typeof(ScoreReferee)} failed to handle score because the operation was cancelled.");
                }
            }
        }

        #endregion
    }
}