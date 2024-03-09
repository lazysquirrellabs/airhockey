using System;
using System.Threading;
using LazySquirrelLabs.AirHockey.Movement;
using LazySquirrelLabs.AirHockey.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LazySquirrelLabs.AirHockey.Match
{
    /// <summary>
    /// A <see cref="Player"/> entity in the <see cref="Match"/>. Controls movement and its scene elements. 
    /// </summary>
    internal class PlayerController : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private InputMovementController _movementController;

        #endregion

        #region Fields

        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private Transform _transform;

        #endregion

        #region Setup

        private void Awake()
        {
            _transform = transform;
        }

        private void OnDestroy()
        {
	        _cancellationTokenSource.Cancel();
	        _cancellationTokenSource.Dispose();
        }

        #endregion

        #region Internal

        /// <summary>
        /// Enables user input to control the player.
        /// </summary>
        internal void StartMoving()
        {
            _movementController.CanMove = true;
        }

        /// <summary>
        /// Stops the player, ignoring any user input.
        /// </summary>
        internal void StopMoving()
        {
            _movementController.CanMove = false;
        }

        /// <summary>
        /// Moves the <see cref="Player"/> instantly.
        /// </summary>
        /// <param name="position">The position to move to.</param>
        internal void MoveTo(Vector3 position)
        {
            _transform.position = position;
        }
        
        /// <summary>
        /// Moves the <see cref="Player"/> asynchronously.
        /// </summary>
        /// <param name="position">The position to move to.</param>
        /// <param name="duration">The duration of the movement, in seconds.</param>
        /// <param name="token">The token for operation cancellation.</param>
        /// <returns>The awaitable task.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="duration"/>
        /// is negative.</exception>
        internal async UniTask MoveToAsync(Vector3 position, float duration, CancellationToken token)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration must be positive.");
            
            var totalTime = 0f;
            var initialPosition = _transform.position;
            var unifiedToken = token.Unify(_cancellationTokenSource.Token);
            while (totalTime <= duration)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, unifiedToken);
                _transform.position = Vector3.Lerp(initialPosition, position, totalTime/duration);
                totalTime += Time.deltaTime;
            }
        }

        #endregion
    }
}