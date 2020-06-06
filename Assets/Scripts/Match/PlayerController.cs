using System.Threading;
using AirHockey.Movement;
using UniRx.Async;
using UnityEngine;

namespace AirHockey.Match
{
    public enum Player
    {
        LeftPlayer,
        RightPlayer
    }
    
    public class PlayerController : MonoBehaviour
    {
        #region Serialized fields

        [SerializeField] private InputMovementController _movementController;

        #endregion

        #region Fields

        private Transform _transform;

        #endregion

        #region Setup

        private void Awake()
        {
            _transform = transform;
        }

        #endregion

        #region Public

        public void StartMoving()
        {
            _movementController.CanMove = true;
        }

        public void StopMoving()
        {
            _movementController.CanMove = false;
        }

        public void MoveTo(Vector3 position)
        {
            _transform.position = position;
        }
        
        public async UniTask MoveToAsync(Vector3 position, float duration, CancellationToken token)
        {
            var totalTime = 0f;
            var initialPosition = _transform.position;
            while (totalTime <= duration)
            {
                await UniTask.Yield();
                token.ThrowIfCancellationRequested();
                _transform.position = Vector3.Lerp(initialPosition, position, totalTime/duration);
                totalTime += Time.deltaTime * 1_000;
            }
        }

        #endregion
    }
}