namespace Domain
{
    class NoVictoryStrategy : IVictoryStrategy
    {
        public EResult CheckResult(Round round)
        {
            return EResult.Inconclusive;
        }
        public string GetWinner(Round round)
        {
            if (CheckResult(round) == EResult.Winner)
                return round.VoteOptions.First(vos => vos.Score == round.VoteOptions.Max(vos => vos.Score)).Name;

            return "No winner";
        }
    }

}