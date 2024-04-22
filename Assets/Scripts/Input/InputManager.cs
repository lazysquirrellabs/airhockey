using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LazySquirrelLabs.AirHockey.Input
{
	/// <summary>
	/// Manages non-pointer (touchscreen on device, clicks on the editor) input.
	/// </summary>
	internal class InputManager : MonoBehaviour
	{
		#region Events

		/// <summary>
		/// Invoked whenever the a back action (e.g. Android's back button) is invoked.
		/// </summary>
		internal event Action OnBack;

		#endregion

		#region Fields

		private Controls _controls;

		#endregion

		#region Setup

		private void Awake()
		{
			_controls = new Controls();
			_controls.Enable();
			_controls.General.Back.performed += HandleBack;
			_controls.General.Enable();
		}

		private void OnDestroy()
		{
			_controls.General.Disable();
			_controls.General.Back.performed -= HandleBack;
			_controls.Disable();
		}

		#endregion

		#region Event handlers

		private void HandleBack(InputAction.CallbackContext _)
		{
			OnBack?.Invoke();
		}

		#endregion
	}
}