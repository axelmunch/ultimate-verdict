namespace Domain
{
    class ELOVoteStrategy : IVerifyVotetrategy
    {
        public void CheckVote(List<Decision> decisions)
        {
            if (decisions == null || decisions.Count % 2 == 0 || decisions.Count(d => d.Score > 0) != decisions.Count(d => d.Score < 0))
            {
                throw new ArgumentException("Erreur dans le vote envoyé");
            }
        }
    }
}
