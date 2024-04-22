using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using LazySquirrelLabs.AirHockey.Match.Referees;
using LazySquirrelLabs.AirHockey.Match.Scoring;
using LazySquirrelLabs.AirHockey.UI.Popups;
using LazySquirrelLabs.AirHockey.Utils;
using UnityEngine;

namespace LazySquirrelLabs.AirHockey.Match.Managers
{
	/// <summary>
	/// A <see cref="Match"/>'s manager.
	/// </summary>
	internal class MatchManager : MonoBehaviour
	{
		#region Events

		internal event Action OnRestartRequest;
		internal event Action OnLeaveRequest;

		#endregion

		#region Serialized fields

		[SerializeField] private PlayerController _leftPlayer;
		[SerializeField] private PlayerController _rightPlayer;
		[SerializeField] private ScoreManager _scoreManager;
		[SerializeField] private PlacementManager _placementManager;
		[SerializeField] private AudioManager _audioManager;
		[SerializeField] private AnnouncementBoard _announcementBoard;
		[SerializeField] private Timer _timer;
		[SerializeField] private ChoicePopup _endMatchPopup;
		[SerializeField, Range(0, 10)] private int _matchStartDelay;
		[SerializeField, Range(0, 10)] private int _celebrationDuration;
		[SerializeField, Range(0, 10)] private int _resetDuration;
		[SerializeField, Range(0, 10)] private int _preparationDuration;
		[SerializeField, Range(0, 10)] private float _endMatchDelay;

		#endregion

		#region Fields

		private readonly CancellationTokenSource _cancellationTokenSource = new();
		private Referee _referee;
		private Score _score;

		#endregion

		#region Setup

		private void Awake()
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			_scoreManager.OnScore += HandleScore;
			_endMatchPopup.OnSelectFirstChoice += HandleRestartChoice;
			_endMatchPopup.OnSelectSecondChoice += HandleLeaveChoice;
		}

		private void OnDestroy()
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
			_referee?.LeaveMatch();
			_endMatchPopup.OnSelectFirstChoice -= HandleRestartChoice;
			_endMatchPopup.OnSelectSecondChoice -= HandleLeaveChoice;
		}

		#endregion

		#region Event handlers

		private async void HandleScore(Player player, Score score)
		{
			_score = score;
			_audioManager.PlayGoal();

			try
			{
				await _referee.ProcessScoreAsync(player, score, _cancellationTokenSource.Token);
			}
			catch (OperationCanceledException)
			{
				Debug.Log("Stopped handling score because the operation was cancelled.");
			}
		}

		private void HandleRestartChoice()
		{
			OnRestartRequest?.Invoke();
		}

		private void HandleLeaveChoice()
		{
			OnLeaveRequest?.Invoke();
		}

		#endregion

		#region Internal

		/// <summary>
		/// Starts a match asynchronously.
		/// </summary>
		/// <param name="settings">The match settings to be used.</param>
		/// <param name="token">Token used for task cancellation.</param>
		/// <returns>An awaitable task representing the entire match setup process.</returns>
		/// <exception cref="NotImplementedException">Thrown whenever an invalid match <see cref="MatchMode"/> is provided
		/// in the <paramref name="settings"/>.</exception>
		internal async UniTask StartMatchAsync(MatchSettings settings, CancellationToken token)
		{
			var info = settings.Value;
			_placementManager.StartMatch();
			var unifiedToken = token.Unify(_cancellationTokenSource.Token);
			await _announcementBoard.AnnounceMatchStartAsync(_matchStartDelay, unifiedToken);
			await _announcementBoard.AnnounceGetReadyAsync(_preparationDuration, unifiedToken);
			Debug.Log($"Starting match on {settings.Mode}, value: {info}");

			switch (settings.Mode)
			{
				case MatchMode.HighScore:
					_referee = new HighScoreReferee(ScoreAndResetAsync, EndAsync, info);
					break;
				case MatchMode.BestOfScore:
					_referee = new BestOfScoreReferee(ScoreAndResetAsync, EndAsync, info);
					break;
				case MatchMode.Time:
					_timer.Show(info);
					var seconds = info * 60;
					var timedReferee = new TimeReferee(ScoreAndResetAsync, EndAsync, seconds, _timer.SetTime);
					timedReferee.StartTimer().Forget();
					_referee = timedReferee;
					break;
				case MatchMode.Endless:
					_referee = new EndlessReferee(ScoreAndResetAsync, EndAsync);
					break;
				default:
					throw new NotImplementedException($"Mode not implemented: {settings.Mode}");
			}

			_audioManager.PlayBuzz();
			_leftPlayer.StartMoving();
			_rightPlayer.StartMoving();
			await _announcementBoard.FadeOutAsync(unifiedToken);
		}

		/// <summary>
		/// Forces the match to stop, asynchronously.
		/// </summary>
		/// <param name="fadeOutDuration">How long the stopping should take, in seconds.</param>
		/// <param name="token">Token used for task cancellation.</param>
		/// <returns>The awaitable task representing the stop process.</returns>
		internal async UniTask StopMatchAsync(float fadeOutDuration, CancellationToken token)
		{
			_placementManager.StopAll();
			_audioManager.PlayBuzz();
			var unifiedToken = token.Unify(_cancellationTokenSource.Token);
			await _audioManager.FadeOutAllAsync(fadeOutDuration, unifiedToken);
		}

		#endregion

		#region Private

		/// <summary>
		/// Scores a goal and resets the rink for the next point, asynchronously.
		/// </summary>
		/// <param name="player">The player who scored</param>
		/// <param name="token">Token used for task cancellation.</param>
		/// <returns>An awaitable task representing the score announcement and the rink resetting.</returns>
		private async UniTask ScoreAndResetAsync(Player player, CancellationToken token)
		{
			_leftPlayer.StopMoving();
			_rightPlayer.StopMoving();
			var unifiedToken = token.Unify(_cancellationTokenSource.Token);
			await _announcementBoard.AnnounceGoalAsync(player, _celebrationDuration, unifiedToken);
			await _placementManager.ResetPlayersAsync(_resetDuration, unifiedToken);
			_placementManager.PlacePuck(player);
			await _announcementBoard.AnnounceGetReadyAsync(_preparationDuration, unifiedToken);
			_audioManager.PlayBuzz();
			_leftPlayer.StartMoving();
			_rightPlayer.StartMoving();
			await _announcementBoard.FadeOutAsync(unifiedToken);
		}

		/// <summary>
		/// End the match instantly.
		/// </summary>
		private async UniTask EndAsync()
		{
			Debug.Log("Match is over");
			_placementManager.StopAll();
			_audioManager.PlayBuzz();
			var token = _cancellationTokenSource.Token;
			await _announcementBoard.AnnounceEndOfMatchAsync(_score.FinalResult, token);
			var delayMilli = (int)(_endMatchDelay * 1_000);
			await UniTask.Delay(delayMilli, DelayType.Realtime, PlayerLoopTiming.Update, token);
			_endMatchPopup.Message = _score.FinalResult switch
			{
				MatchResult.Tie            => "It's a tie!!",
				MatchResult.LeftPlayerWin  => $"Player 1 wins!\n{GetScoreMessage(_score)}",
				MatchResult.RightPlayerWin => $"Player 2 wins!\n{GetScoreMessage(_score)}",
				_                          => throw new ArgumentOutOfRangeException()
			};
			_endMatchPopup.Show();
			return;

			string GetScoreMessage(Score score) => $"({score.LeftPlayer} x {score.RightPlayer})";
		}

		#endregion
	}
}