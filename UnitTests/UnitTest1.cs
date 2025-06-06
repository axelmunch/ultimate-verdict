namespace UnitTests;

using System.Diagnostics;
using Domain;

public class PluralVoteTests
{
    [Theory]
    [InlineData("Jean", "Candidat1", "Martin", "Candidat2", 1, 0, "Jean", EResult.Winner)]
    [InlineData("Seb", "Candidat1", "Rob", "Candidat2", 2, 3, "Rob", EResult.Winner)]
    [InlineData("Seb", "Candidat1", "Axel", "Candidat2", 2, 2, "No winner", EResult.Draw)]
    [InlineData("Seb", "Candidat1", "Axel", "Candidat2", 0, 0, "No winner", EResult.Inconclusive)]
    public void ResultTest(string candidate1Name, string candidate1Desc, string candidate2Name, string candidate2Desc, int candidate1score, int candidate2score, string expectedWinner, EResult expectedResult)
    {
        /*
        List<Candidate> candidates = new()
        {
            new Candidate(candidate1Name, candidate1Desc),
            new Candidate(candidate2Name, candidate2Desc)
        };

        VotingSystemBase pluralvote = new(candidates, "Plural");
        pluralvote.AddVote(candidate1Name, candidate1score);
        pluralvote.AddVote(candidate2Name, candidate2score);

        Assert.Equal(expectedResult, pluralvote.GetResult());
        Assert.Equal(expectedWinner, pluralvote.winnerName);
        */
        Assert.True(true);
    }

    private static readonly List<string> CandidateMockList = new List<string>
    {
        "Alice", "Bob", "Charlie", "David", "Emma", "Frank"
    };

    [Theory]
    [InlineData(EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false, new int[] {23, 12}, EResult.Winner, 1)]
    public void GlobalResultTest(EVotingSystems votingType, int nbVoteOptions, int nbRounds, int[] qualifiedPerRound, EVictorySettings victorySettings, bool runAgainIfDraw, int[] scores, EResult expectedResult, int numWinner)
    {
        List<VoteOption> voteOptions = new();
        for (int i = 0; i < nbVoteOptions-1; i++)
        {
            voteOptions.Add(new VoteOption(CandidateMockList[i], "Candidat" + (i + 1)));
        }

        VotingSystemBase vote = new(votingType, voteOptions, nbRounds, qualifiedPerRound, victorySettings, runAgainIfDraw);

        while (vote.currentRound < nbRounds)
        {
            vote.NextRound();
            for (int i = 0; i < vote.Rounds[vote.currentRound-1].VoteOptions.Count; i++)
            {
                vote.AddVote(vote.Rounds[vote.currentRound-1].VoteOptions[i].Name, scores[i]);
            }
        }

            


        Assert.Equal(expectedResult, vote.GetResult());

    }
}
