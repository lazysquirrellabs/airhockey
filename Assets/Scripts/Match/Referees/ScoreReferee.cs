using System;
using AirHockey.Match.Managers;

namespace AirHockey.Match.Referees
{
    public abstract class ScoreReferee : Referee
    {
        #region Delegates

        protected delegate bool ScoreCheck(Score score);

        #endregion   
        
        #region Fields

        private readonly uint _highScore;
        private readonly Action _stopListening;
        private readonly ScoreCheck _isOver;

        #endregion

        #region Setup
        
        protected ScoreReferee(Action pause, Action<Player> resume, Action end, ScoreCheck isOver, ScoreManager manager) 
            : base(pause, resume, end)
        {
            _stopListening = () => manager.OnScore -= HandleScore;
            _isOver = isOver;
            manager.OnScore += HandleScore;
        }
        
        #endregion
        
        #region Event hanlders

        private void HandleScore(Player player, Score score)
        {
            Pause();
            if (_isOver(score)) 
                End();
            else
                Resume(player);
        }

        #endregion

        #region Public

        public override void CancelMatch()
        {
            _stopListening?.Invoke();
        }

        #endregion
    }
}