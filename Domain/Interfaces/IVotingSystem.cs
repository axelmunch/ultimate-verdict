namespace Domain
{
    public interface IVotingSystem
    {
        void AddCandidate(int id, string candidateName);
        void AddDecision(Decision decision, int? roundNumber = null);
        EResult GetRoundResult(int roundNumber);
        bool NextRound();
    }
}
