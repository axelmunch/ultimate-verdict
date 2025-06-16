namespace Domain
{
    class ELOVoteStrategy : IVerifyVotetrategy
    {
        public void CheckVote(List<Decision> decisions)
        {
            if (decisions == null || decisions.Count != 2 || decisions[1].Score > 0)
            {
                throw new ArgumentException("Erreur dans le vote envoyé");
            }
        }
    }
}
