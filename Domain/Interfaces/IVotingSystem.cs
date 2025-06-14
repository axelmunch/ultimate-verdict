namespace Domain
{
    public interface IVotingSystem
    {
        void AddDecision(Decision decision, int? roundNumber = null);
        EResult GetRoundResult(int roundNumber);
        bool NextRound();
    }
}
