namespace Domain
{
    public interface IVotingSystem
    {
        void AddCandidate(string candidateName, string candidateDescription);
        void AddVote(string canditateName, int scoreToAdd);
        EResult GetResult();
        void NextRound();
    }
}