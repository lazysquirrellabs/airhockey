using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LazySquirrelLabs.AirHockey.Movement
{
	/// <summary>
	/// Moves a 2D object around based on pointer drag movement.
	/// </summary>
	internal class InputMovementController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		#region Serialized fields

		[SerializeField] private Rigidbody2D _rigidBody;
		[SerializeField, Range(1, 200)] private float _maximumSpeed;

		#endregion

		#region Fields

		private bool _dragging;
		private Vector2 _position;
		private bool _canMove;

		#endregion

		#region Properties

		/// <summary> Fetches the current mouse world position, abstracting the implementation. </summary>
		internal Func<Vector2, Vector2> GetMousePosition { private get; set; }

		/// <summary>
		/// Whether the game object can be moved via input.
		/// </summary>
		internal bool CanMove
		{
			set
			{
				if (value == _canMove)
				{
					return;
				}

				_canMove = value;
				_position = _rigidBody.position;
				_rigidBody.velocity = Vector2.zero;
			}
		}

		#endregion

		#region Update

		private void FixedUpdate()
		{
			if (!_dragging || !_canMove)
			{
				return;
			}

			// Ensure that the applied translation doesn't exceed the maximum speed
			var currentPosition = _rigidBody.position;
			var distance = Vector2.Distance(_position, currentPosition);
			var desiredSpeed = distance / Time.fixedDeltaTime;
			var deltaSpeed = Time.fixedDeltaTime * Mathf.Min(desiredSpeed, _maximumSpeed);
			var direction = (_position - currentPosition).normalized;
			var position = currentPosition + deltaSpeed * direction;
			_rigidBody.MovePosition(position);
		}

		#endregion

		#region Event handlers

		public void OnDrag(PointerEventData eventData)
		{
			if (_canMove)
			{
				_position = GetMousePosition(eventData.position);
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (_canMove)
			{
				_dragging = true;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (_canMove)
			{
				_dragging = false;
			}
		}

		#endregion
	}
}