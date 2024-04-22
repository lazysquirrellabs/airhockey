using System;
using Cysharp.Threading.Tasks;

namespace LazySquirrelLabs.AirHockey.Match.Referees
{
	/// <summary>
	/// <see cref="ScoreReferee"/> which implements the rules of the "high score" mode. See
	/// <see cref="MatchMode.HighScore"/> for more.
	/// </summary>
	internal class HighScoreReferee : ScoreReferee
	{
		#region Setup

		/// <summary>
		/// <see cref="HighScoreReferee"/>'s constructor.
		/// </summary>
		/// <param name="pause">How to pause the match when a player scores.</param>
		/// <param name="endAsync">How to end the match.</param>
		/// <param name="score">The high score used to end the match.</param>
		internal HighScoreReferee(AsyncPauser pause, Func<UniTask> endAsync, uint score)
			: base(pause, endAsync, IsOver(score)) { }

		#endregion

		#region Private

		/// <summary>
		/// Gets a <see cref="ScoreReferee.ScoreCheck"/> based on the "high score" rule.
		/// </summary>
		/// <param name="highScore">The number of goals scored by one player which will end the match.</param>
		/// <returns>The score checker which ends the match whenever <paramref name="highScore"/> goals have been
		/// scored by a player. </returns>
		private static ScoreCheck IsOver(uint highScore)
		{
			return s => s.LeftPlayer >= highScore || s.RightPlayer >= highScore;
		}

		#endregion
	}
}