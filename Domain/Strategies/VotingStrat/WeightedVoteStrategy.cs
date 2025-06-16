namespace Domain
{
    class WeightedVoteStrategy : IVerifyVotetrategy

    {
        public void CheckVote(List<Decision> decisions)
        {
            if (decisions.Count <= 1 || decisions.Select(v => v.Id).Distinct().Count() != decisions.Count || decisions.Any(v => v.Score < 0))
            {
                throw new ArgumentException("Erreur dans le vote envoyé");
            }
        }
    }
}
