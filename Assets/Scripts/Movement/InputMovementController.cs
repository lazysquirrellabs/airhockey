using AirHockey.Input;
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

        #endregion

        #region Properties

        /// <summary> Fetches the current mouse world position, abstracting the implementation. </summary>
        public InputManager.MouseWorldPositionGetter GetMousePosition { private get; set; }

        #endregion

        #region Update

        private void FixedUpdate()
        {
            if (!_dragging) return;
            
            _rigidBody.MovePosition(_position);
        }

        #endregion
        
        #region Event handlers
        
        public void OnDrag(PointerEventData eventData)
        {
            _position = GetMousePosition();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _dragging = false;
        }
        
        #endregion
    }
}