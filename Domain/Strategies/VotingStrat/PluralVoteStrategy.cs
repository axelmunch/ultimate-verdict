namespace Domain
{
    class PluralVoteStrategy : IVerifyVotetrategy
    {
        public void CheckVote(List<Decision> decisions)
        {
            if (decisions.Count != 1 || decisions[0].Score != 1)
            {
                throw new ArgumentException("Erreur dans le vote envoyé");
            }
        }
    }
}
