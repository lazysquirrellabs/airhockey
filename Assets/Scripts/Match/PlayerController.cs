using AirHockey.Movement;
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

        public void Regroup(Vector2 position)
        {
            _transform.position = position;
        }

        #endregion
    }
}