namespace AirHockey.Match
{
    /// <summary>
    /// Settings used to start a new valid <see cref="Match"/>.
    /// </summary>
    public readonly struct MatchSettings
    {
        #region Properties

        public MatchMode Mode { get; }
        /// <summary>
        /// Extra information that might be used by some <see cref="MatchMode"/>s. Its meaning varies on the mode.
        /// </summary>
        public uint Value { get; }

        #endregion

        #region Setup

        /// <summary>
        /// <see cref="MatchSettings"/>'s constructor used whenever there is some extra information besides the
        /// <see cref="MatchMode"/>.
        /// </summary>
        /// <param name="mode">The match mode.</param>
        /// <param name="value">Extra information used by the =<paramref name="mode"/>.</param>
        public MatchSettings(MatchMode mode, uint value)
        {
            Mode = mode;
            Value = value;
        }

        /// <summary>
        /// <see cref="MatchSettings"/>'s constructor used whenever there is no extra information besides the
        /// <see cref="MatchMode"/>.
        /// </summary>
        /// <param name="mode">The match mode.</param>
        public MatchSettings(MatchMode mode)
        {
            Mode = mode;
            Value = 0;
        }

        #endregion
    }
}