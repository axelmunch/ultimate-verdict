namespace Domain
{
    public interface IVotingSystem
    {
        void AddDecision(List<Decision> decisions, int? roundNumber = null);
        EResult GetLastRoundResult(int roundNumber);
        bool NextRound();
    }
}
