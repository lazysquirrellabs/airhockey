using LazySquirrelLabs.AirHockey.Movement;
using UnityEngine;

namespace LazySquirrelLabs.AirHockey.Match.Managers
{
	/// <summary>
	/// Manages non-UI game input like pointer movement.
	/// </summary>
	internal class InputManager : MonoBehaviour
	{
		#region Serialized fields

		[SerializeField] private UnityEngine.Camera _camera;
		[SerializeField] private InputMovementController[] _mousePositionListeners;

		#endregion

		#region Setup

		private void Awake()
		{
			// Setup the mouse listener's delegate.
			foreach (var listener in _mousePositionListeners)
			{
				listener.GetMousePosition = GetMouseWorldPosition2D;
			}
		}

		#endregion

		#region Private

		/// <summary>
		/// Fetches the mouse position in the 2D world based on the <see cref="InputManager"/>'s camera.
		/// </summary>
		/// <returns>The mouse world 2D position.</returns>
		private Vector2 GetMouseWorldPosition2D(Vector2 screenPos)
		{
			var pos3D = _camera.ScreenToWorldPoint(screenPos);
			return new Vector2(pos3D.x, pos3D.y);
		}

		#endregion
	}
}