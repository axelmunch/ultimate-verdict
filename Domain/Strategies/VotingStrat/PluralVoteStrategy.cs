namespace Domain
{
    class PluralVoteStrategy : IVerifyVotetrategy
    {
        public void CheckVote(List<Decision> cadidate, List<int> scores)
        {
            if (cadidate.Count > 1 || scores.Count > 1 || scores.Count != cadidate.Count || scores[0] != 1)
            {
                throw new ArgumentException("Erreur dans le vote envoyé");
            }
        }
    }
}
