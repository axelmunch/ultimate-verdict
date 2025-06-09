namespace Domain
{
    public interface IVictoryStrategy
    {
        public EResult CheckResult(Round round);
        public List<int> GetWinner(Round round);
    }
}
