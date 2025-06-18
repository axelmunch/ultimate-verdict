namespace Domain
{
    public class TwoThirdsMajorityStrategy : IVictoryStrategy
    {
        public int nbVotes;
        public int maxScore;
        int countMax;
        public EResult CheckResult(List<Option> options)
        {
            if (options.Count == 0)
                return EResult.Inconclusive;

            nbVotes = options.Sum(v => v.Score);

            maxScore = options.Max(vos => vos.Score);
            countMax = options.Count(vos => vos.Score == maxScore);

            if (maxScore == 0)
                return EResult.Inconclusive;

            if (countMax > 1)
                return EResult.Draw;

            if (countMax == 1 && maxScore >= nbVotes * 2 / 3)
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
