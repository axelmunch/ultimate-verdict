namespace Domain
{
    public interface IVotinSystemStrategy
    {
        public void AddVote(string canditateName, int scoreToAdd);
    }
}
