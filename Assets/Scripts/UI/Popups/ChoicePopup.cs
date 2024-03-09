using System;
using UnityEngine;
using UnityEngine.UI;

namespace LazySquirrelLabs.AirHockey.UI.Popups
{
	internal class ChoicePopup : Popup
	{
		#region Events

		internal event Action OnSelectFirstChoice;

		internal event Action OnSelectSecondChoice;

		#endregion

		#region Serialized fields

		[SerializeField] private Button _firstChoiceButton;
		[SerializeField] private Button _secondChoiceButton;

		#endregion

		#region Setup

		protected void Awake()
		{
			_firstChoiceButton.onClick.AddListener(HandleSelectFirstChoice);
			_secondChoiceButton.onClick.AddListener(HandleSelectSecondChoice);
		}

		protected void OnDestroy()
		{
			_firstChoiceButton.onClick.RemoveListener(HandleSelectFirstChoice);
			_secondChoiceButton.onClick.RemoveListener(HandleSelectSecondChoice);
		}

		#endregion

		#region Event handlers

		private void HandleSelectFirstChoice()
		{
			OnSelectFirstChoice?.Invoke();
		}

		private void HandleSelectSecondChoice()
		{
			OnSelectSecondChoice?.Invoke();
		}

		#endregion
	}
}