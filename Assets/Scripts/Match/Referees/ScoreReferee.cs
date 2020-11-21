using System;
using UnityEngine;

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