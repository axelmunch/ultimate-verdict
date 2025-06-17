namespace Domain
{
    public interface IVotingSystem
    {
        void AddDecision(List<Decision> decisions, int? roundNumber = null);
        EResult GetRoundResult(int roundNumber);
        bool NextRound();
    }
}
