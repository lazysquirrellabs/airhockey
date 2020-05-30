using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private Goal _player1Goal;
        [SerializeField] private Goal _player2Goal;
        [SerializeField] private Text _scoreText;

        #endregion

        #region Fields

        private uint _player1Score;
        private uint _player2Score;

        #endregion

        #region Setup

        private void Awake()
        {
            _player1Goal.OnScore += ScoreRight;
            _player2Goal.OnScore += ScoreLeft;
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
            _player2Score++;
            UpdateScore();
        }
        
        private void ScoreRight()
        {
            _player1Score++;
            UpdateScore();
        }

        #endregion

        #region Private

        private void UpdateScore()
        {
            _scoreText.text = $"{_player1Score}-{_player2Score}";
        }

        #endregion

    }
}