namespace Domain
{
    public interface IVotingSystem
    {
        void AddCandidate(string candidateName, int id);
        void AddVote(int id, int scoreToAdd);
        EResult GetRoundResult(int roundNumber);
        bool NextRound();
    }
}
