namespace Domain
{
    public interface IVerifyVotetrategy
    {
        public void CheckVote(List<Decision> decisions, bool singleDecision);
    }
}
