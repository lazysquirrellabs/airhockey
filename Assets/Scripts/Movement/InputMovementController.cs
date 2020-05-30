using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AirHockey.Movement
{
    /// <summary>
    /// Moves a 2D object around based on pointer drag movement.
    /// </summary>
    public class InputMovementController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        #region Serialized fields

        [SerializeField] private Rigidbody2D _rigidBody;

        #endregion

        #region Fields

        private bool _dragging;
        private Vector2 _position;
        private bool _canMove;

        #endregion

        #region Properties

        /// <summary> Fetches the current mouse world position, abstracting the implementation. </summary>
        public Func<Vector2> GetMousePosition { private get; set; }

        public bool CanMove
        {
            set
            {
                if (value == _canMove) return;

                _canMove = value;
                _position = _rigidBody.position;
                _rigidBody.velocity = Vector2.zero;
            }
        }

        #endregion

        #region Update

        private void FixedUpdate()
        {
            if (!_dragging || !_canMove) return;
            
            _rigidBody.MovePosition(_position);
        }

        #endregion
        
        #region Event handlers
        
        public void OnDrag(PointerEventData eventData)
        {
            if (_canMove)
                _position = GetMousePosition();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_canMove)
                _dragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_canMove)
                _dragging = false;
        }
        
        #endregion
    }
}