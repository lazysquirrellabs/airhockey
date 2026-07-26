using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LazySquirrelLabs.AirHockey.Localization;
using LazySquirrelLabs.AirHockey.Match.Scoring;
using LazySquirrelLabs.AirHockey.Utils;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UniTaskExtensions = LazySquirrelLabs.AirHockey.Utils.UniTaskExtensions;

namespace LazySquirrelLabs.AirHockey.Match
{
	/// <summary>
	/// Board seen in the match which gives general visual announcements (e.g. score, match start and end).
	/// </summary>
	internal class AnnouncementBoard : MonoBehaviour
	{
		#region Serialized fields

		[SerializeField] private CanvasGroup _canvas;
		[SerializeField] private LocalizeStringEvent _leftLocalizer;
		[SerializeField] private LocalizeStringEvent _rightLocalizer;
		[Header("Localization entries")]
		[SerializeField] private LocalizedString _matchStartLocalization;
		[SerializeField] private LocalizedString _scoredLocalization;
		[SerializeField] private LocalizedString _otherScoredLocalization;
		[SerializeField] private LocalizedString _getReadyLocalization;
		[SerializeField] private LocalizedString _goLocalization;
		[SerializeField] private LocalizedString _youWinLocalization;
		[SerializeField] private LocalizedString _youLoseLocalization;
		[SerializeField] private LocalizedString _tieLocalization;
			
		#endregion

		#region Fields

		/// <summary> Duration in seconds of general fade outs used in FadeOutAsync. </summary>
		private const float FadeOutDuration = 1f;
		private const float MatchStartFadeDuration = 0.5f;
		private const float MatchEndFadeDuration = 0.5f;
		// Localization
		private const string ValueKey = "value";
		private StringVariable _leftVariable;
		private StringVariable _rightVariable;

		#endregion

		#region Fields

		private readonly CancellationTokenSource _cancellationTokenSource = new();

		#endregion

		#region Setup

		private void Awake()
		{
			_leftVariable = _leftLocalizer.GetVariable<StringVariable>(ValueKey);
			_rightVariable = _rightLocalizer.GetVariable<StringVariable>(ValueKey);
		}

		private void OnDestroy()
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
		}

		#endregion

		#region Internal

		/// <summary>
		/// Displays a "match is starting..." announcement asynchronously.
		/// </summary>
		/// <param name="duration">The duration of the announcement.</param>
		/// <param name="token">The token for operation cancellation.</param>
		/// <returns>The awaitable task.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="duration"/>
		/// is negative.</exception>
		internal async UniTask AnnounceMatchStartAsync(int duration, CancellationToken token)
		{
			if (duration < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration must be positive.");
			}

			_leftLocalizer.Localize(_matchStartLocalization);
			_rightLocalizer.Localize(_matchStartLocalization);
			SetLocalizationVariable(duration);
			var unifiedToken = token.Unify(_cancellationTokenSource.Token);
			await FadeInAsync(MatchStartFadeDuration, unifiedToken);
			_canvas.alpha = 1f;

			while (duration > 0)
			{
				SetLocalizationVariable(duration);
				await UniTask.Delay(1_000, false, PlayerLoopTiming.Update, unifiedToken);
				duration--;
			}

			await FadeOutAsync(MatchStartFadeDuration, unifiedToken);
			return;

			void SetLocalizationVariable(int seconds)
			{
				_leftVariable.Value = seconds.ToString();
				_rightVariable.Value = seconds.ToString();
			}
		}

		/// <summary>
		/// Announces that a goal has been scored asynchronously.
		/// </summary>
		/// <param name="player">The <see cref="Player"/> that scored the goal.</param>
		/// <param name="duration">Teh duration of the announcement, in seconds.</param>
		/// <param name="token">The token for operation cancellation.</param>
		/// <returns>The awaitable task.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="duration"/>
		/// is negative.</exception>
		/// <exception cref="NotImplementedException">Thrown if an invalid <see cref="Player"/>
		/// was provided.</exception>
		internal async UniTask AnnounceGoalAsync(Player player, int duration, CancellationToken token)
		{
			if (duration < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration must be positive.");
			}

			switch (player)
			{
				case Player.LeftPlayer:
					_leftLocalizer.Localize(_scoredLocalization);
					_rightLocalizer.Localize(_otherScoredLocalization);
					_rightVariable.Value = 1.ToString();
					break;
				case Player.RightPlayer:
					_rightLocalizer.Localize(_scoredLocalization);
					_leftLocalizer.Localize(_otherScoredLocalization);
					_leftVariable.Value = 2.ToString();
					break;
				default:
					throw new NotImplementedException($"Player not valid: {player}");
			}

			var unifiedToken = token.Unify(_cancellationTokenSource.Token);
			await FadeInAsync(duration * 0.1f, unifiedToken);
			await UniTask.Delay((int)(duration * 1_000 * 0.8f), false, PlayerLoopTiming.Update, unifiedToken);
			await FadeOutAsync(duration * 0.1f, unifiedToken);
		}

		/// <summary>
		/// Displays a "get ready" announcement to the players asynchronously.
		/// </summary>
		/// <param name="duration">The duration of the announcement, in seconds.</param>
		/// <param name="token">The token for operation cancellation.</param>
		/// <returns>The awaitable task.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="duration"/>
		/// is negative.</exception>
		internal async UniTask AnnounceGetReadyAsync(int duration, CancellationToken token)
		{
			if (duration < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(duration), duration, "Duration must be positive.");
			}

			_leftLocalizer.Localize(_getReadyLocalization);
			_rightLocalizer.Localize(_getReadyLocalization);
			var unifiedToken = token.Unify(_cancellationTokenSource.Token);
			await FadeInAsync(duration * 0.1f, unifiedToken);
			await UniTask.Delay((int)(duration * 1_000 * 0.9f), false, PlayerLoopTiming.Update, unifiedToken);
			_leftLocalizer.Localize(_goLocalization);
			_rightLocalizer.Localize(_goLocalization);
		}

		/// <summary>
		/// Fades the board out, regardless of what's been shown.
		/// </summary>
		/// <param name="token">The token for operation cancellation.</param>
		/// <returns>The awaitable task.</returns>
		internal async UniTask FadeOutAsync(CancellationToken token)
		{
			var unifiedToken = token.Unify(_cancellationTokenSource.Token);
			await FadeOutAsync(FadeOutDuration, unifiedToken);
		}

		internal async UniTask AnnounceEndOfMatchAsync(MatchResult matchResult, CancellationToken token)
		{
			switch (matchResult)
			{
				case MatchResult.Tie:
					_leftLocalizer.Localize(_tieLocalization);
					_rightLocalizer.Localize(_tieLocalization);
					break;
				case MatchResult.LeftPlayerWin:
					_leftLocalizer.Localize(_youWinLocalization);
					_rightLocalizer.Localize(_youLoseLocalization);
					break;
				case MatchResult.RightPlayerWin:
					_leftLocalizer.Localize(_youLoseLocalization);
					_rightLocalizer.Localize(_youWinLocalization);
					break;
				default:
					throw new NotImplementedException($"Result not valid: {matchResult}");
			}

			var unifiedToken = token.Unify(_cancellationTokenSource.Token);
			await FadeInAsync(MatchEndFadeDuration, unifiedToken);
		}

		#endregion

		#region Private

		private void SetAlpha(float alpha) => _canvas.alpha = alpha;

		/// <summary>
		/// Fades the announcement board out asynchronously.
		/// </summary>
		/// <param name="duration">The duration of the fade, in seconds.</param>
		/// <param name="token">The token for operation cancellation.</param>
		/// <returns>The awaitable task.</returns>
		private async UniTask FadeOutAsync(float duration, CancellationToken token)
		{
			await UniTaskExtensions.ProgressAsync(SetAlpha, 1f, 0f, duration, token);
		}

		/// <summary>
		/// Fades the announcement board in asynchronously.
		/// </summary>
		/// <param name="duration">The duration of the fade, in seconds.</param>
		/// <param name="token">The token for operation cancellation.</param>
		/// <returns>The awaitable task.</returns>
		private async UniTask FadeInAsync(float duration, CancellationToken token)
		{
			await UniTaskExtensions.ProgressAsync(SetAlpha, 0f, 1f, duration, token);
		}

		#endregion
	}
}