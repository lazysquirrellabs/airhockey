using System;
using Cysharp.Threading.Tasks;

namespace AirHockey.Match.Referees
{
    public abstract class ScoreReferee : Referee
    {
        #region Delegates

        protected delegate bool ScoreCheck(Score score);

        #endregion   
        
        #region Fields

        private readonly uint _highScore;
        private readonly ScoreCheck _isOver;

        #endregion

        #region Setup
        
        protected ScoreReferee(Pauser pause, Action end, ScoreCheck isOver, Action<Scorer> subscribe) 
            : base(pause, end, subscribe)
        {
            _isOver = isOver;
        }
        
        #endregion
        
        #region Event hanlders

        protected override void HandleScore(Player player, Score score)
        {
            if (_isOver(score)) 
                End();
            else
                Pause(player).Forget();
        }

        #endregion
    }
}