namespace LazySquirrelLabs.AirHockey.Match
{
	/// <summary>
	/// The modes that a <see cref="Match"/> can be.
	/// </summary>
	internal enum MatchMode
	{
		/// <summary>
		/// The first player to score a given number of goals wins. The match might NOT end in a tie.
		/// </summary>
		HighScore,

		/// <summary>
		/// Stops the match whenever a given number of total goals has been scored. The match result is based on which
		/// player has scored. The match might end in a tie.
		/// </summary>
		BestOfScore,

		/// <summary>
		/// The match ends whenever a given amount of time has been elapsed. The match might end in a tie.
		/// </summary>
		Time,

		/// <summary>
		/// The match never ends unless the players leave it.
		/// </summary>
		Endless
	}
}