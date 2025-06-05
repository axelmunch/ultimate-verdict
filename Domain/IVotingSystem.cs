namespace Domain
{
    public interface IVotingSystem
    {
        string Name { get; }
        void AddCandidate(string candidateName, string candidateDescription);
        void AddVote(string canditateName, int scoreToAdd);
        void GetStanding();
        void GetResult();
    }
}