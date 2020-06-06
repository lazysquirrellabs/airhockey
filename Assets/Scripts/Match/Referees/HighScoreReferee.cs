using System;
using AirHockey.Match.Managers;

namespace AirHockey.Match.Referees
{
    public class HighScoreReferee : ScoreReferee
    {
        #region Setup

        public HighScoreReferee(Action pause, Action<Player> resume, Action end, ScoreManager scoreManager, uint score) 
            : base(pause, resume, end, IsOver(score), scoreManager)
        {
        }

        #endregion

        #region Private

        private static ScoreCheck IsOver(uint highScore)
        {
            return s => s.LeftPlayer >= highScore || s.RightPlayer >= highScore;
        }

        #endregion
    }
}