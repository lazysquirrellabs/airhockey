using System;
using UnityEngine;

namespace AirHockey.Match.Referees
{
    public class BestOfScoreReferee : ScoreReferee
    {
        #region Setup

        public BestOfScoreReferee(Pauser pause, Action end, Action<Scorer> subscribeToScore, uint score) 
            : base(pause, end, IsOver(score), subscribeToScore)
        {
        }

        #endregion

        #region Private

        private static ScoreCheck IsOver(uint maxScore)
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