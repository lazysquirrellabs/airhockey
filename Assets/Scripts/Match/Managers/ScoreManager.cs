using AirHockey.Match.Scoring;
using UnityEngine;
using UnityEngine.UI;

namespace AirHockey.Match.Managers
{
    /// <summary>
    /// Manages all the scoring in a <see cref="Match"/>.
    /// </summary>
    internal class ScoreManager : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// Invoked whenever a player scores.
        /// </summary>
        internal event Scorer OnScore;

        #endregion
        
        #region Serialized fields

        [SerializeField] private Goal _player1Goal;
        [SerializeField] private Goal _player2Goal;
        [SerializeField] private Text _scoreText;

        #endregion

        #region Fields

        /// <summary>
        ///  The current score.
        /// </summary>
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
        
        /// <summary>
        /// Scores for the player on the left side of the rink.
        /// </summary>
        private void ScoreLeft()
        {
            _score.ScoreGoal(Player.LeftPlayer);
            UpdateScoreVisuals();
            OnScore?.Invoke(Player.LeftPlayer, _score);
        }
        
        /// <summary>
        /// Scores for the player on the right side of the rink.
        /// </summary>
        private void ScoreRight()
        {
            _score.ScoreGoal(Player.RightPlayer);
            UpdateScoreVisuals();
            OnScore?.Invoke(Player.RightPlayer, _score);
        }

        #endregion

        #region Private

        /// <summary>
        /// Updates the visuals that represent the match score.
        /// </summary>
        private void UpdateScoreVisuals()
        {
            _scoreText.text = $"{_score.LeftPlayer} {_score.RightPlayer}";
        }

        #endregion
    }
}