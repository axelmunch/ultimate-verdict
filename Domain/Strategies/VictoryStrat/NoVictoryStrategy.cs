namespace Domain
{
    public class NoVictoryStrategy : IVictoryStrategy
    {
        public EResult CheckResult(List<Option> options)
        {
            return EResult.None;
        }

        public List<int> GetWinner(List<Option> options)
        {
            return options.Select(v => v.Id).ToList();
        }

        public List<Option> GetRoundStanding(List<Option> options)
        {
            return options
                .ToList();
        }
    }

}
