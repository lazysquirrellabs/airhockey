using AirHockey.Movement;
using UnityEngine;

namespace AirHockey.Input
{
    /// <summary>
    /// Manages non-UI game input like pointer movement.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public delegate Vector2 MouseWorldPositionGetter();
        
        #region Serialized fields

        [SerializeField] private Camera _camera;
        [SerializeField] private InputMovementController[] _mousePositionListeners;

        #endregion

        #region Setup

        private void Awake()
        {
            // Setup the mouse listener's delegate.
            foreach (var listener in _mousePositionListeners)
                listener.GetMousePosition = GetMouseWorldPosition2D;
        }

        #endregion

        #region Private

        /// <summary>
        /// Fetches the mouse position in the 2D world based on the <see cref="InputManager"/>'s camera.
        /// </summary>
        /// <returns>The mouse world 2D position.</returns>
        private Vector2 GetMouseWorldPosition2D()
        {
            var pos3D = _camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            return new Vector2(pos3D.x, pos3D.y);
        }

        #endregion
    }
}