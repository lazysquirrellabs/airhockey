using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AirHockey.Match.Managers
{
    /// <summary>
    /// Manages the placement of <see cref="Match"/> elements (i.e. players and puck).
    /// </summary>
    internal class PlacementManager : MonoBehaviour
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

        #region Internal

        /// <summary>
        /// Places the match elements for a match start.
        /// </summary>
        internal void StartMatch()
        {
            _leftPlayer.StopMoving();
            _rightPlayer.StopMoving();
            _puck.Regroup(_puckNeutralStart.position);
            _leftPlayer.MoveTo(_leftPlayerStart.position);
            _rightPlayer.MoveTo(_rightPlayerStart.position);
        }

        /// <summary>
        /// Resets the <see cref="Player"/>s position, asynchronously.
        /// </summary>
        /// <param name="duration">The duration of the position placement, in seconds.</param>
        /// <param name="token">The token for operation cancellation.</param>
        /// <returns>The awaitable task.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="duration"/>
        /// is negative.</exception>
        internal async UniTask ResetPlayersAsync(float duration, CancellationToken token)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration must be positive.");
            
            var leftWait = _leftPlayer.MoveToAsync(_leftPlayerStart.position, duration, token);
            var rightWait = _rightPlayer.MoveToAsync(_rightPlayerStart.position, duration, token);
            await UniTask.WhenAll(leftWait, rightWait);
        }

        /// <summary>
        /// Blocks players and puck from moving.
        /// </summary>
        internal void StopAll()
        {
            _leftPlayer.StopMoving();
            _rightPlayer.StopMoving();
            _puck.StopMoving();
        }

        /// <summary>
        /// Places the puck on a <paramref name="player"/> initial shot position.
        /// </summary>
        /// <param name="player">The players who will receive the puck.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if an invalid <paramref name="player"/>
        /// is given.</exception>
        internal void PlacePuck(Player player)
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