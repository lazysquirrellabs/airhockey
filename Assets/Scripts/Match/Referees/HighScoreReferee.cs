using System;

namespace AirHockey.Match.Referees
{
    public class HighScoreReferee : ScoreReferee
    {
        #region Setup

        public HighScoreReferee(Pauser pause, Action end, Action<Scorer> subscribeToScore, uint score) 
            : base(pause, end, IsOver(score), subscribeToScore)
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