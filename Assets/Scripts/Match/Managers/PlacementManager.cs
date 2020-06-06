using System;
using System.Threading;
using UniRx.Async;
using UnityEngine;

namespace AirHockey.Match.Managers
{
    public class PlacementManager : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private PlayerController _leftPlayer;
        [SerializeField] private PlayerController _rightPlayer;
        [SerializeField] private Puck _puck;
        [SerializeField] private Transform _leftPlayerStart;
        [SerializeField] private Transform _rightPlayerStart;
        [SerializeField] private Transform _puckNeutralStart;
        [SerializeField] private Transform _puckLeftStart;
        [SerializeField] private Transform _puckRightStart;

        #endregion

        #region Public

        public void StartMatch()
        {
            _puck.Regroup(_puckNeutralStart.position);
            _leftPlayer.MoveTo(_leftPlayerStart.position);
            _rightPlayer.MoveTo(_rightPlayerStart.position);
        }

        public async UniTask ResetPlayersAsync(float duration, CancellationToken token)
        {
            var leftWait = _leftPlayer.MoveToAsync(_leftPlayerStart.position, duration, token);
            var rightWait = _rightPlayer.MoveToAsync(_rightPlayerStart.position, duration, token);
            await UniTask.WhenAll(leftWait, rightWait);
        }

        public void PlacePuck(Player player)
        {
            switch (player)
            {
                case Player.LeftPlayer:
                    _puck.Regroup(_puckLeftStart.position);
                    break;
                case Player.RightPlayer:
                    _puck.Regroup(_puckRightStart.position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(player), player, null);
            } 
        }

        #endregion
    }
}