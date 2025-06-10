namespace Domain
{
    class AbsoluteMajorityStrategy : IVictoryStrategy
    {
        private int nbVotes;
        private int maxScore;
        public EResult CheckResult(List<VoteOption> voteOptions)
        {
            if (voteOptions.Count == 0)
                return EResult.Inconclusive;

            nbVotes = voteOptions.Sum(v => v.Score);

            maxScore = voteOptions.Max(vos => vos.Score);
            int countMax = voteOptions.Count(vos => vos.Score == maxScore);

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
        public List<int> GetWinner(List<VoteOption> voteOptions)
        {
            if (CheckResult(voteOptions) == EResult.Winner)
                return new List<int> { voteOptions.First(v => v.Score == maxScore).Id };

            if (CheckResult(voteOptions) == EResult.Draw)
                return voteOptions.Where(v => v.Score == maxScore).Select(v => v.Id).ToList();


            return voteOptions.Select(v => v.Id).ToList();
        }
    }

}
