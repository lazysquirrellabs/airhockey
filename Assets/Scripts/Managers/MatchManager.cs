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
        [SerializeField] private AnnouncementBoard _announcementBoard;
        [SerializeField, Range(0, 10)] private int _matchStartDelay;
        [SerializeField, Range(0, 10)] private int _goalCelebrationDuration;
        [SerializeField, Range(0, 10)] private int _regroupDuration;

        #endregion

        #region Private



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
            var playerID = player == Player.LeftPlayer ? 1 : 2;
            await _announcementBoard.AnnouncePlayerScored(playerID, _goalCelebrationDuration);
            await UniTask.Delay(_goalCelebrationDuration * 1_000);
            _placementManager.Regroup(player);
            await _announcementBoard.AnnounceGetReadyAsync(_regroupDuration);
            _leftPlayer.StartMoving();
            _rightPlayer.StartMoving();
        }

        #endregion

        #region Private

        private async void StartMatch()
        {
            _leftPlayer.StopMoving();
            _rightPlayer.StopMoving();
            _placementManager.StartMatch();
            await _announcementBoard.AnnounceMatchStartAsync(_matchStartDelay);
            await _announcementBoard.AnnounceGetReadyAsync(_regroupDuration);
            _leftPlayer.StartMoving();
            _rightPlayer.StartMoving();
        }

        #endregion
    }
}