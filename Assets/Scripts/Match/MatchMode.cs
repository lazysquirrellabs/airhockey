using System;

namespace AirHockey.Match
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

    internal static class MatchModeExtensions
    {
        #region Internal
        
        /// <summary>
        /// Fetches the name of the extra information needed by a <paramref name="matchMode"/> to be valid.
        /// </summary>
        /// <param name="matchMode">The mode.</param>
        /// <returns>A friendly name for the extra information.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown whenever the given <paramref name="matchMode"/>
        /// does not require extra information.</exception>
        /// <exception cref="NotImplementedException">Thrown whenever the given <paramref name="matchMode"/> has not
        /// been implemented yet.</exception>
        internal static string InfoName(this MatchMode matchMode)
        {
            switch (matchMode)
            {
                case MatchMode.HighScore:
                case MatchMode.BestOfScore:
                    return "score";
                case MatchMode.Time:
                    return "duration";
                case MatchMode.Endless:
                    const string message = "Endless mode doesn't require info.";
                    throw new ArgumentOutOfRangeException(nameof(matchMode), matchMode, message);
                default:
                    throw new NotImplementedException($"Mode not implemented: {matchMode}.");
            }
        }
        
        /// <summary>
        /// Fetches the units of the extra information needed by a <paramref name="matchMode"/> to be valid.
        /// </summary>
        /// <param name="matchMode">The mode.</param>
        /// <returns>A friendly name for the extra information's unit.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown whenever the given <paramref name="matchMode"/>
        /// does not require extra information.</exception>
        /// <exception cref="NotImplementedException">Thrown whenever the given <paramref name="matchMode"/> has not
        /// been implemented yet.</exception>
        internal static string InfoUnitName(this MatchMode matchMode)
        {
            switch (matchMode)
            {
                case MatchMode.HighScore:
                case MatchMode.BestOfScore:
                    return "points";
                case MatchMode.Time:
                    return "minutes";
                case MatchMode.Endless:
                    const string message = "Endless mode doesn't require info.";
                    throw new ArgumentOutOfRangeException(nameof(matchMode), matchMode, message);
                default:
                    throw new NotImplementedException($"Mode not implemented: {matchMode}.");
            }
        }
        
        #endregion
    }
}