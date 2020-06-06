using System;
using AirHockey.Match.Managers;
using UnityEngine;

namespace AirHockey.Match.Referees
{
    public class BestOfScoreReferee : ScoreReferee
    {
        #region Setup

        public BestOfScoreReferee(Action pause, Action<Player> resume, Action end, ScoreManager scoreManager, uint score) 
            : base(pause, resume, end, IsOver(score), scoreManager)
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