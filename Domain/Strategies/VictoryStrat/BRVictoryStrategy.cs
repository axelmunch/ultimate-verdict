namespace Domain
{
    public class BRVictoryStrategy : IVictoryStrategy
    {
        private int maxScore;
        public EResult CheckResult(List<Option> options)
        {
            if (options.Count == 0)
                return EResult.Inconclusive;

            if (options.Count > 2)
                return EResult.None;

            maxScore = options.Max(vos => vos.Score);
            int countMax = options.Count(vos => vos.Score == maxScore);

            if (maxScore == 0)
                return EResult.Inconclusive;

            if (countMax > 1)
                return EResult.Draw;

            if (countMax == 1)
            {
                return EResult.Winner;
            }

            return EResult.Inconclusive;
        }

        public List<int> GetWinner(List<Option> options)
        {
            if (CheckResult(options) == EResult.Winner)
                return new List<int> { options.First(v => v.Score == maxScore).Id };

            if (CheckResult(options) == EResult.Draw)
                return options.Where(v => v.Score == maxScore).Select(v => v.Id).ToList();


            return options.Select(v => v.Id).ToList();
        }

        public List<Option> GetRoundStanding(List<Option> options)
        {
            return options
                .OrderByDescending(vo => vo.Score)
                .ToList();
        }
    }
}
