using AirHockey.Match;
using UniRx.Async;
using UnityEngine;

namespace AirHockey.Managers
{
    public class MatchManager : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private PlayerController _leftPlayer;
        [SerializeField] private PlayerController _rightPlayer;
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private PlacementManager _placementManager;
        [SerializeField, Range(0, 10)] private int _resetDuration;
        [SerializeField, Range(0, 10)] private int _regroupDuration;

        #endregion

        #region Setup

        private void Awake()
        {
            _scoreManager.OnScore += HandleScore;
        }
        
        private void Start()
        {
            // TODO: Remove this once the basic game loop is implemented
            StartMatch();
        }

        private void OnDestroy()
        {
            _scoreManager.OnScore -= HandleScore;
        }

        #endregion

        #region Event handlers

        private async void HandleScore(Player player)
        {
            _leftPlayer.StopMoving();
            _rightPlayer.StopMoving();
            Debug.Log($"{player} scored.");
            await UniTask.Delay(_resetDuration * 1_000);
            _placementManager.Regroup(player);
            Debug.Log("On your marks...");
            await UniTask.Delay(_regroupDuration * 1_000);
            Debug.Log("GO!");
            _leftPlayer.StartMoving();
            _rightPlayer.StartMoving();
        }

        #endregion

        #region Private

        private async void StartMatch()
        {
            Debug.Log("Starting match...");
            _leftPlayer.StopMoving();
            _rightPlayer.StopMoving();
            _placementManager.StartMatch();
            await UniTask.Delay(_regroupDuration * 1_000);
            Debug.Log("GO!");
            _leftPlayer.StartMoving();
            _rightPlayer.StartMoving();
        }

        #endregion
    }
}