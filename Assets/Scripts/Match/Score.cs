using System;

namespace AirHockey.Match
{
    public struct Score
    {
        public enum Result
        {
            Tie,
            LeftPlayerWin,
            RightPlayerWin
        }
        
        #region Properties    

        public uint LeftPlayer { get; private set; }
        public uint RightPlayer { get; private set; }

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