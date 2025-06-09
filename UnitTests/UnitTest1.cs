namespace UnitTests;

using System.Diagnostics;
using Domain;

public class PluralVoteTests
{
    private static readonly List<string> CandidateMockList = new List<string>
    {
        "Alice", "Bob", "Charlie", "David", "Emma", "Frank"
    };

    private static readonly List<int> CandidateMockListID = new List<int>
    {
        8765, 9255, 6123, 4534, 2563, 1892
    };

    [Theory]
    //Majorité relative
    [InlineData(EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false, new int[] { 23, 22 }, EResult.Winner, "Alice")]
    [InlineData(EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false, new int[] { 23, 23 }, EResult.Draw, "Alice, Bob")]
    [InlineData(EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false, new int[] { 0, 0 }, EResult.Inconclusive, "Alice, Bob")]
    [InlineData(EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false, new int[] { 93, 42, 17, 98, 10, 140, 134 }, EResult.Winner, "David")]
    [InlineData(EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false, new int[] { 93, 42, 17, 98, 10, 140, 154 }, EResult.Winner, "Alice")]
    [InlineData(EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false, new int[] { 93, 42, 17, 98, 10, 154, 154 }, EResult.Draw, "David, Alice")]
    //[InlineData(EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, true, new int[] { 93, 42, 17, 98, 10, 154, 154, 140, 154 }, EResult.Winner, "Alice")]

    //Majorité absolue
    [InlineData(EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.Absolute_Majority, false, new int[] { 93, 42, 17, 98, 10 }, EResult.Inconclusive, "Alice, Bob, Charlie, David, Emma")]
    [InlineData(EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.Absolute_Majority, false, new int[] { 43, 11, 17, 21, 188 }, EResult.Winner, "Emma")]
    [InlineData(EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.Absolute_Majority, false, new int[] { 43, 123, 17, 21, 123 }, EResult.Draw, "Bob, Emma")]

    //Majorité 2/3
    [InlineData(EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.TwoThirds_Majority, false, new int[] { 93, 42, 17, 98, 10 }, EResult.Inconclusive, "Alice, Bob, Charlie, David, Emma")]
    [InlineData(EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.TwoThirds_Majority, false, new int[] { 33, 11, 218, 7, 11 }, EResult.Winner, "Charlie")]
    [InlineData(EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.TwoThirds_Majority, false, new int[] { 43, 123, 17, 21, 123 }, EResult.Draw, "Bob, Emma")]
    public void GlobalResultTest(EVotingSystems votingType, int nbVoteOptions, int nbRounds, int[] qualifiedPerRound, EVictorySettings victorySettings, bool runAgainIfDraw, int[] scores, EResult expectedResult, string expectedWinnerName)
    {
        List<VoteOption> voteOptions = new();
        for (int i = 0; i < nbVoteOptions; i++)
        {
            voteOptions.Add(new VoteOption(CandidateMockList[i], "Candidat" + (i + 1), CandidateMockListID[i]));
        }

        VotingSystemBase vote = new(votingType, voteOptions, nbRounds, qualifiedPerRound, victorySettings, runAgainIfDraw);

        while (vote.NextRound())
        {
            for (int i = 0; i < vote.Rounds[vote.currentRound - 1].VoteOptions.Count; i++)
            {
                if (vote.currentRound == 1)
                    vote.AddVote(vote.Rounds[vote.currentRound - 1].VoteOptions[i].Id, scores[i]);
                else
                {
                    vote.AddVote(vote.Rounds[vote.currentRound - 1].VoteOptions[i].Id, scores[i + nbVoteOptions * (vote.currentRound - 1)]);
                }
            }
        }

        Assert.Equal(expectedResult, vote.GetRoundResult(vote.currentRound));
        Assert.Equal(expectedWinnerName, vote.GetVoteWinner());
    }









    public static IEnumerable<object[]> StructuredCandidateTestData =>
    new List<object[]>
    {
        // Test 1 – majorité relative simple, Alice gagne
        new object[]
        {
            EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice", new List<int> { 93 }),
                new(9255, "Bob",   new List<int> { 12 })
            },
            EResult.Winner,
            "Alice"
        },

        // Test 2 – égalité
        new object[]
        {
            EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice", new List<int> { 50 }),
                new(9255, "Bob",   new List<int> { 50 })
            },
            EResult.Draw,
            "Alice, Bob"
        },

        // Test 3 – Inconclusif
        new object[]
        {
            EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice", new List<int> { 0 }),
                new(9255, "Bob",   new List<int> { 0 })
            },
            EResult.Inconclusive,
            "Alice, Bob"
        },

        // Test 4 – deux tours, Alice bat David
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 154 }),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98, 140 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            "Alice"
        },

        // Test 5 – deux tours, egalité
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 140 }),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98, 140 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Draw,
            "David, Alice"
        },

        // Test 5 – deux tours, egalité puis victoire
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 140, 145 }),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98, 140, 142 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            "Alice"
        }
    };


    [Theory]
    [MemberData(nameof(StructuredCandidateTestData))]
    public void GlobalResultTest_WithStructuredCandidates(EVotingSystems votingType, int nbVoteOptions, int nbRounds, int[] qualifiedPerRound, EVictorySettings victorySettings, bool runAgainIfDraw, List<TestCandidate> candidates, EResult expectedResult, string expectedWinnerName)
    {
        var voteOptions = candidates
            .Take(nbVoteOptions)
            .Select(c => new VoteOption(c.Name, c.Description, c.Id))
            .ToList();

        var vote = new VotingSystemBase(votingType, voteOptions, nbRounds, qualifiedPerRound, victorySettings, runAgainIfDraw);

        while (vote.NextRound())
        {
            foreach (var vo in vote.Rounds[vote.currentRound - 1].VoteOptions)
            {
                var testData = candidates.First(c => c.Id == vo.Id);
                vote.AddVote(vo.Id, testData.ScoresPerRound[vote.currentRound - 1]);
            }
        }

        Assert.Equal(expectedResult, vote.GetRoundResult(vote.currentRound));
        Assert.Equal(expectedWinnerName, vote.GetVoteWinner());
    }
}
