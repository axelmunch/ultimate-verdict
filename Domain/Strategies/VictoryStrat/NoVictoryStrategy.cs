namespace Domain
{
    class NoVictoryStrategy : IVictoryStrategy
    {
        public EResult CheckResult(List<VoteOption> voteOptions)
        {
            return EResult.None;
        }

        public List<int> GetWinner(List<VoteOption> voteOptions)
        {
            return voteOptions.Select(v => v.Id).ToList();
        }
    }

}
