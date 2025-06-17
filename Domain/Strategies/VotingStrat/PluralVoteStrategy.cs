namespace Domain
{
    class PluralVoteStrategy : IVerifyVotetrategy
    {
        public void CheckVote(List<Decision> decisions, bool singleDecision)
        {
            if (singleDecision && decisions.Count != 1)
                throw new ArgumentException("Erreur dans le vote envoyé");

            if (decisions.Any(v => v.Score != 1))
            {
                throw new ArgumentException("Erreur dans le vote envoyé");
            }
        }
    }
}
