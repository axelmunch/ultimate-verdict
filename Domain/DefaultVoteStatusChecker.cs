namespace Domain
{
    public class DefaultVoteStatusChecker : IVoteStatusChecker
    {
        public bool IsVoteOver(int currentRound, int totalRounds, EVictorySettings victoryType, bool runAgainIfDraw, Func<int, EResult> getRoundResult)
        {
            bool isLastRound = currentRound >= totalRounds;
            bool isMajorityVictory = victoryType == EVictorySettings.Absolute_Majority || victoryType == EVictorySettings.TwoThirds_Majority;

            return
                (isLastRound && !runAgainIfDraw) ||
                (currentRound > 0 && isMajorityVictory && getRoundResult(currentRound) == EResult.Winner) ||
                (currentRound == totalRounds && getRoundResult(currentRound) == EResult.Winner);
        }
    }
}
