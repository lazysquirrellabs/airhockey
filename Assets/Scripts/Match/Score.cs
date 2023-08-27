using System;

namespace AirHockey.Match
{
    /// <summary>
    /// A <see cref="Match"/> score.
    /// </summary>
    internal struct Score
    {
        #region Properties    

        /// <summary>
        /// How many goals the left player has scored
        /// </summary>
        internal uint LeftPlayer { get; private set; }
        /// <summary>
        /// How many goals the right player has scored
        /// </summary>
        internal uint RightPlayer { get; private set; }

        /// <summary>
        /// The final result of the <see cref="Match"/> if this was its final score.
        /// </summary>
        internal Result FinalResult
        {
            get
            {
                if (LeftPlayer == RightPlayer)
                    return Result.Tie;
                if (LeftPlayer > RightPlayer)
                    return Result.LeftPlayerWin;
                return Result.RightPlayerWin;
            }
        }

        #endregion

        #region Internal

        /// <summary>
        /// Register a goal scored by the given <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The player who scored the goal.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown whenever the given <see cref="Player"/> is out of
        /// range. </exception>
        internal void ScoreGoal(Player player)
        {
            switch (player)
            {
                case Player.LeftPlayer:
                    LeftPlayer++;
                    break;
                case Player.RightPlayer:
                    RightPlayer++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(player), player, null);
            }
        }

        #endregion
    }
}