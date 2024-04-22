using System;
using System.Threading;
using LazySquirrelLabs.AirHockey.Utils;
using UnityEngine;
using UniTask = Cysharp.Threading.Tasks.UniTask;

namespace LazySquirrelLabs.AirHockey.Match.Managers
{
	/// <summary>
	/// A match's audio manager.
	/// </summary>
	internal class AudioManager : MonoBehaviour
	{
		#region Serialized fields

		[SerializeField] private AudioSource _loop;
		[SerializeField] private AudioSource _goalCrowd;
		[SerializeField] private AudioSource _goalHorn;
		[SerializeField] private AudioSource _buzz;

		#endregion

		#region Fields

		private readonly CancellationTokenSource _cancellationTokenSource = new();

		#endregion

		#region Setup

		private async void Awake()
		{
			try
			{
				await _loop.FadeInAsync(1f, 3f, _cancellationTokenSource.Token);
			}
			catch (OperationCanceledException)
			{
				Debug.Log("Audio manager started because the operation was cancelled.");
			}
		}

		private void OnDestroy()
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
		}

		#endregion

		#region Internal

		/// <summary>
		/// Fades all match audio out.
		/// </summary>
		/// <param name="duration">The duration of the fade in seconds.</param>
		/// <param name="token">Token used for task cancellation.</param>
		/// <returns>The awaitable task.</returns>
		internal async UniTask FadeOutAllAsync(float duration, CancellationToken token)
		{
			var unifiedToken = token.Unify(_cancellationTokenSource.Token);
			var goalHorn = _goalHorn.FadeOutAsync(duration, unifiedToken);
			var goalCrowd = _goalCrowd.FadeOutAsync(duration, unifiedToken);
			var loop = _loop.FadeOutAsync(duration, unifiedToken);
			await UniTask.WhenAll(goalHorn, goalCrowd, loop);
		}

		/// <summary>
		/// Plays the sound effects for when a player scores.
		/// </summary>
		internal void PlayGoal()
		{
			_goalCrowd.Play();
			_goalHorn.Play();
		}

		/// <summary>
		/// Plays a buzz sound.
		/// </summary>
		internal void PlayBuzz()
		{
			_buzz.Play();
		}

		#endregion
	}
}