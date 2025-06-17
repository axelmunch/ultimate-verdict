namespace Domain
{
    class RankedVoteStrategy : IVerifyVotetrategy
    {
        public void CheckVote(List<Decision> decisions)
        {
            if (
                decisions == null || decisions.Count <= 1 ||
                decisions.Any(v => v.Score < 0) ||
                decisions.Select(v => v.Score).Distinct().Count() != decisions.Count ||
                decisions.Select(v => v.Id).Distinct().Count() != decisions.Count ||
                decisions.Any(v => v.Score > decisions.Select(v => v.Id).Distinct().Count())
            )
            {

                throw new ArgumentException("Erreur dans le vote envoyé");
            }
        }
    }
}
