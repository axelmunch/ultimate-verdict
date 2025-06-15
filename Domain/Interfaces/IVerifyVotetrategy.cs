namespace Domain
{
    public interface IVerifyVotetrategy
    {
        public void CheckVote(List<Decision> cadidate, List<int> scores);
    }
}
