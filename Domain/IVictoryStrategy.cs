namespace Domain
{
    public interface IVictoryStrategy
    {
        public EResult CheckResult(Round round);
        public string GetWinner(Round round);
    }
}