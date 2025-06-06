namespace UnitTests;

using System.Diagnostics;
using Domain;

public class PluralVoteTests
{
    private static readonly List<string> CandidateMockList = new List<string>
    {
        "Alice", "Bob", "Charlie", "David", "Emma", "Frank"
    };

    [Theory]
    [InlineData(EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false, new int[] { 23, 12 }, EResult.Winner, "Alice")]
    [InlineData(EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false, new int[] { 23, 23 }, EResult.Draw, "No winner")]
    [InlineData(EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false, new int[] { 0, 0 }, EResult.Inconclusive, "No winner")]
    [InlineData(EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false, new int[] { 93, 42, 17, 98, 10, 140, 134 }, EResult.Winner, "David")]
    [InlineData(EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false, new int[] { 93, 42, 17, 98, 10, 140, 154 }, EResult.Winner, "Alice")]
    public void GlobalResultTest(EVotingSystems votingType, int nbVoteOptions, int nbRounds, int[] qualifiedPerRound, EVictorySettings victorySettings, bool runAgainIfDraw, int[] scores, EResult expectedResult, string expectedWinnerName)
    {
        List<VoteOption> voteOptions = new();
        for (int i = 0; i < nbVoteOptions; i++)
        {
            voteOptions.Add(new VoteOption(CandidateMockList[i], "Candidat" + (i + 1)));
        }

        VotingSystemBase vote = new(votingType, voteOptions, nbRounds, qualifiedPerRound, victorySettings, runAgainIfDraw);

        while (vote.currentRound < nbRounds)
        {
            vote.NextRound();
            for (int i = 0; i < vote.Rounds[vote.currentRound - 1].VoteOptions.Count; i++)
            {
                if (vote.currentRound == 1)
                    vote.AddVote(vote.Rounds[vote.currentRound - 1].VoteOptions[i].Name, scores[i]);
                else
                {
                    vote.AddVote(vote.Rounds[vote.currentRound - 1].VoteOptions[i].Name, scores[i + nbVoteOptions * (vote.currentRound - 1)]);
                }
            }
        }

        Assert.Equal(expectedResult, vote.GetResult(nbRounds));
        Assert.Equal(expectedWinnerName, vote.winnerName);
    }
}
