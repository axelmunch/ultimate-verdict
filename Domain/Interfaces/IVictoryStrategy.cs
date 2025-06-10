namespace Domain
{
    public interface IVictoryStrategy
    {
        public EResult CheckResult(List<VoteOption> voteOptions);
        public List<int> GetWinner(List<VoteOption> voteOptions);
    }
}
