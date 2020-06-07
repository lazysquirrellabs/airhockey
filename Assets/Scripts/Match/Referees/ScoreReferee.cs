using System;
using AirHockey.Match.Managers;
using UniRx.Async;

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
        
        protected ScoreReferee(Action pause, Resumer resume, Action end, ScoreCheck isOver, ScoreManager manager) 
            : base(pause, resume, end, manager)
        {
            _isOver = isOver;
        }
        
        #endregion
        
        #region Event hanlders

        protected override void HandleScore(Player player, Score score)
        {
            Pause();
            if (_isOver(score)) 
                End();
            else
                Resume(player).Forget();
        }

        #endregion
    }
}