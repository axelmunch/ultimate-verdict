namespace Domain
{
    public static class VictoryStrategyFactory
    {
        public static IVictoryStrategy Create(EVictorySettings type)
        {
            return type switch
            {
                EVictorySettings.Relative_Majority => new RelativeMajorityStrategy(),
                EVictorySettings.Absolute_Majority => new AbsoluteMajorityStrategy(),
                EVictorySettings.TwoThirds_Majority => new TwoThirdsMajorityStrategy(),
                EVictorySettings.None => new NoVictoryStrategy(),
                EVictorySettings.LastManStanding => new BRVictoryStrategy(),
                _ => throw new ArgumentException("Unsupported victory setting", nameof(type)),
            };
        }
    }
}
