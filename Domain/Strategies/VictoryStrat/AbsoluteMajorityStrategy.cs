namespace Domain
{
    class AbsoluteMajorityStrategy : IVictoryStrategy
    {
        private int nbVotes;
        private int maxScore;
        public EResult CheckResult(Round round)
        {
            if (round.VoteOptions.Count == 0)
                return EResult.Inconclusive;

            nbVotes = round.VoteOptions.Sum(v => v.Score);

            maxScore = round.VoteOptions.Max(vos => vos.Score);
            int countMax = round.VoteOptions.Count(vos => vos.Score == maxScore);

            if (maxScore == 0)
                return EResult.Inconclusive;

            if (countMax > 1)
                return EResult.Draw;

            if (countMax == 1 && maxScore >= nbVotes / 2)
            {
                return EResult.Winner;
            }

            return EResult.Inconclusive;
        }
        public string GetWinner(Round round)
        {
            if (CheckResult(round) == EResult.Winner)
                return round.VoteOptions.First(vos => vos.Score == maxScore).Name;

            if (CheckResult(round) == EResult.Draw)
                return string.Join(", ", round.VoteOptions.Where(v => v.Score == maxScore).Select(v => v.Name));


            return string.Join(", ", round.VoteOptions.Select(v => v.Name));
        }
    }

}