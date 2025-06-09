namespace Domain
{
    class NoVictoryStrategy : IVictoryStrategy
    {
        public EResult CheckResult(Round round)
        {
            return EResult.None;
        }

        public string GetWinner(Round round)
        {
            return string.Join(", ", round.VoteOptions.Select(v => v.Name));
        }
    }

}
