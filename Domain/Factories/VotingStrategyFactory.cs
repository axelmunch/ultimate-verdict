namespace Domain
{
    public static class VotingStrategyFactory
    {
        public static IVerifyVotetrategy Create(EVotingSystems type)
        {
            return type switch
            {
                EVotingSystems.Plural => new PluralVoteStrategy(),
                EVotingSystems.Weighted => new WeightedVoteStrategy(),
                EVotingSystems.Ranked => new RankedVoteStrategy(),
                EVotingSystems.ELO => new ELOVoteStrategy(),
                _ => throw new ArgumentException("Unsupported voting system", nameof(type)),
            };
        }
    }
}
