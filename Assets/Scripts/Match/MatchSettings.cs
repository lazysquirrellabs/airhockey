namespace AirHockey.Match
{
    public readonly struct MatchSettings
    {
        #region Properties

        public Mode Mode { get; }
        public uint Value { get; }

        #endregion

        #region Setup

        public MatchSettings(Mode mode, uint value)
        {
            Mode = mode;
            Value = value;
        }

        #endregion
    }
}