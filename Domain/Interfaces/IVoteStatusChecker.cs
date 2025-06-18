namespace Domain
{
    public interface IVoteStatusChecker
    {
        bool IsVoteOver(int currentRound, int totalRounds, EVictorySettings victoryType, bool runAgainIfDraw, Func<int, EResult> getRoundResult);
    }
}
