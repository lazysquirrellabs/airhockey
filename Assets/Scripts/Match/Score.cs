using System;

namespace AirHockey.Match
{
    /// <summary>
    /// A <see cref="Match"/> score.
    /// </summary>
    public struct Score
    {
        /// <summary>
        /// Possible outcomes of a finished <see cref="Match"/>.
        /// </summary>
        public enum Result
        {
            Tie,
            LeftPlayerWin,
            RightPlayerWin
        }
        
        #region Properties    

        /// <summary>
        /// How many goals the left player has scored
        /// </summary>
        public uint LeftPlayer { get; private set; }
        /// <summary>
        /// How many goals the right player has scored
        /// </summary>
        public uint RightPlayer { get; private set; }

        /// <summary>
        /// The final result of the <see cref="Match"/> if this was its final score.
        /// </summary>
        public Result FinalResult
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

        #region Public

        /// <summary>
        /// Register a goal scored by the given <paramref name="player"/>.
        /// </summary>
        /// <param name="player">The player who scored the goal.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown whenever the given <see cref="Player"/> is out of
        /// range. </exception>
        public void ScoreGoal(Player player)
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