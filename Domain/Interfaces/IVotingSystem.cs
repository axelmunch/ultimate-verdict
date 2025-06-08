namespace Domain
{
    public interface IVotingSystem
    {
        void AddCandidate(string candidateName, string candidateDescription, int id);
        void AddVote(int id, int scoreToAdd);
        EResult GetResult(int roundNumber);
        void NextRound();
    }
}