namespace Domain
{
    class PluralVoteStrategy : IVerifyVotetrategy
    {
        public void CheckVote(List<Decision> decisions)
        {
            if (decisions.Any(v => v.Score != 1))
            {
                throw new ArgumentException("Erreur dans le vote envoyé");
            }
        }
    }
}
