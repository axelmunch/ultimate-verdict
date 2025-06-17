namespace Domain
{
    public interface IVictoryStrategy
    {
        public EResult CheckResult(List<Option> options);
        public List<int> GetWinner(List<Option> options);
        public List<Option> GetRoundStanding(List<Option> options);
    }
}
