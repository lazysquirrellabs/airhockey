using System;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Match.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Events

        public event Action<Player,Score> OnScore;

        #endregion
        
        #region Serialized fields

        [SerializeField] private Goal _player1Goal;
        [SerializeField] private Goal _player2Goal;
        [SerializeField] private Text _scoreText;

        #endregion

        #region Fields

        private Score _score;

        #endregion

        #region Setup

        private void Awake()
        {
            _player1Goal.OnScore += ScoreRight;
            _player2Goal.OnScore += ScoreLeft;
            _score = new Score();
        }
        
        private void OnDestroy()
        {
            _player1Goal.OnScore -= ScoreRight;
            _player2Goal.OnScore -= ScoreLeft;
        }

        #endregion

        #region Event handlers

        
        private void ScoreLeft()
        {
            _score.ScoreGoal(Player.LeftPlayer);
            UpdateScore();
            OnScore?.Invoke(Player.LeftPlayer, _score);
        }
        
        private void ScoreRight()
        {
            _score.ScoreGoal(Player.RightPlayer);
            UpdateScore();
            OnScore?.Invoke(Player.RightPlayer, _score);
        }

        #endregion

        #region Private

        private void UpdateScore()
        {
            _scoreText.text = $"{_score.LeftPlayer}-{_score.RightPlayer}";
        }

        #endregion

    }
}