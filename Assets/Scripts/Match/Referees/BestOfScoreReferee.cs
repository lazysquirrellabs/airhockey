using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LazySquirrelLabs.AirHockey.Match.Referees
{
    /// <summary>
    /// <see cref="ScoreReferee"/> which implements the rules of the "best of score" mode. See
    /// <see cref="MatchMode.BestOfScore"/> for more.
    /// </summary>
    internal class BestOfScoreReferee : ScoreReferee
    {
        #region Setup

        /// <summary>
        /// <see cref="BestOfScoreReferee"/>'s constructor.
        /// </summary>
        /// <param name="pause">How to pause the match when a player scores.</param>
        /// <param name="endAsync">How to end the match.</param>
        /// <param name="score">The maximum score used to end the match.</param>
        internal BestOfScoreReferee(AsyncPauser pause, Func<UniTask> endAsync, uint score) 
            : base(pause, endAsync, GetBestOfScoreChecker(score))
        {
        }

        #endregion

        #region Private

        /// <summary>
        /// Gets a <see cref="ScoreReferee.ScoreCheck"/> based on the "best of score" rule.
        /// </summary>
        /// <param name="maxScore">The number of goals in total which will end the match.</param>
        /// <returns>The score checker which ends the match whenever <paramref name="maxScore"/> goals have been
        /// scored in total. </returns>
        private static ScoreCheck GetBestOfScoreChecker(uint maxScore)
        {
            return s =>
            {
                if (s.LeftPlayer + s.RightPlayer >= maxScore)
                    return true;

                var limit = Mathf.CeilToInt((float) maxScore / 2);
                
                if (s.LeftPlayer >= limit)
                    return true;
                return s.RightPlayer >= limit;
            };
        }

        #endregion
    }
}