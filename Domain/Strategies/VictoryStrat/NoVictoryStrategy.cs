namespace Domain
{
    class NoVictoryStrategy : IVictoryStrategy
    {
        public EResult CheckResult(Round round)
        {
            return EResult.None;
        }

        public List<int> GetWinner(Round round)
        {
            return round.VoteOptions.Select(v => v.Id).ToList();
        }
    }

}
