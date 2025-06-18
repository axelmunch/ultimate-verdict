namespace UnitTests;

using System.Diagnostics;
using Domain;

public class PluralVoteTests
{

    [Theory]
    [MemberData(nameof(TestData.StructuredCandidateTestData), MemberType = typeof(TestData))]
    public void GlobalResultTest_WithStructuredCandidates(EVotingSystems votingType, int nbRounds, int[] qualifiedPerRound, EVictorySettings victorySettings, bool runAgainIfDraw, List<TestCandidate> candidates, EResult expectedResult, List<int> expectedWinnerID)
    {
        var options = candidates
            .Take(candidates.Count)
            .Select(c => new Option(c.Id, c.Name))
            .ToList();

        var vote = new VotingSystemBase(votingType, options, nbRounds, qualifiedPerRound, victorySettings, runAgainIfDraw, new List<Round>());


        do
        {
            var roundDecisions = new List<Decision>();

            int roundIndex = vote.currentRound - 1;

            foreach (var roundOption in vote.Rounds[roundIndex].Options)
            {
                for (int i = 0; i < candidates.First(c => c.Id == roundOption.Id).ScoresPerRound[roundIndex]; i++)

                    roundDecisions.Add(new Decision(roundOption.Id, 1));
            }

            vote.AddDecision(roundDecisions, vote.currentRound);

        }
        while (vote.NextRound());

        Assert.Equal(expectedResult, vote.GetRoundResult(vote.currentRound));
        Assert.Equal(expectedWinnerID, vote.GetVoteWinner());
    }

    private List<Option> GetValidOptions() => new()
        {
            new Option(1, "Alice"),
            new Option(2, "Bob")
        };

    private int[] GetValidQualifiedPerRound() => new[] { 1 };

    private List<Round> GetEmptyRounds() => new();

    [Fact]
    public void Constructor_Throws_When_LessThanTwoOptions()
    {
        var options = new List<Option> { new Option(1, "OnlyOne") };
        var qualifiedPerRound = GetValidQualifiedPerRound();
        var rounds = GetEmptyRounds();

        var ex = Assert.Throws<ArgumentException>(() =>
            new VotingSystemBase(EVotingSystems.Plural, options, 1, qualifiedPerRound, EVictorySettings.Relative_Majority, false, rounds));
    }

    [Fact]
    public void Constructor_Throws_When_NbRounds_Is_Zero()
    {
        var options = GetValidOptions();
        var qualifiedPerRound = new int[0];
        var rounds = GetEmptyRounds();

        var ex = Assert.Throws<ArgumentException>(() =>
            new VotingSystemBase(EVotingSystems.Plural, options, 0, qualifiedPerRound, EVictorySettings.Relative_Majority, false, rounds));
    }

    [Fact]
    public void Constructor_Throws_When_QualifiedPerRound_Length_Is_Invalid()
    {
        var options = GetValidOptions();
        var qualifiedPerRound = new[] { 1, 2 };
        var rounds = GetEmptyRounds();

        var ex = Assert.Throws<ArgumentException>(() =>
            new VotingSystemBase(EVotingSystems.Plural, options, 1, qualifiedPerRound, EVictorySettings.Relative_Majority, false, rounds));
    }

    [Fact]
    public void Constructor_Throws_When_QualifiedPerRound_Contains_Zero()
    {
        var options = GetValidOptions();
        var qualifiedPerRound = new[] { 0 };
        var rounds = GetEmptyRounds();

        var ex = Assert.Throws<ArgumentException>(() =>
            new VotingSystemBase(EVotingSystems.Plural, options, 1, qualifiedPerRound, EVictorySettings.Relative_Majority, false, rounds));
    }

    [Fact]
    public void AddDecision_Throws_When_RoundNumber_Is_Invalid()
    {
        var options = GetValidOptions();
        var qualifiedPerRound = GetValidQualifiedPerRound();
        var votingSystem = new VotingSystemBase(EVotingSystems.Plural, options, 1, qualifiedPerRound, EVictorySettings.Relative_Majority, false, new());

        var decision = new List<Decision> { new Decision(1, 5) };

        Assert.Throws<ArgumentOutOfRangeException>(() => votingSystem.AddDecision(decision, 10));
    }


    [Fact]
    public void AddDecision_Throws_When_Unknown_Candidate()
    {
        var options = new List<Option>
        {
            new Option(1, "Alice"),
            new Option(2, "Bob"),
            new Option(3, "Charlie")
        };
        var qualifiedPerRound = new[] { 1 };
        var rounds = new List<Round>();

        var votingSystem = new VotingSystemBase(EVotingSystems.Plural, options, 1, qualifiedPerRound, EVictorySettings.Relative_Majority, false, rounds
        );

        var invalidDecision = new List<Decision> { new Decision(99, 5) };

        var ex = Assert.Throws<InvalidOperationException>(() =>
            votingSystem.AddDecision(invalidDecision));
    }


    [Fact]
    public void AddDecision_Throws_When_Candidate_Already_Eliminated()
    {
        var options = new List<Option>
        {
            new Option(1, "Alice"),
            new Option(2, "Bob"),
            new Option(3, "Charlie")
        };
        var qualifiedPerRound = new[] { 2, 1 };
        var rounds = new List<Round>();

        var votingSystem = new VotingSystemBase(EVotingSystems.Plural, options, 2, qualifiedPerRound, EVictorySettings.Relative_Majority, false, rounds
        );

        votingSystem.AddDecision(new List<Decision> { new Decision(1, 1), new Decision(2, 1), new Decision(3, 1), new Decision(1, 1), new Decision(2, 1) });

        votingSystem.NextRound();

        var invalidDecision = new List<Decision> { new Decision(3, 1) };

        var ex = Assert.Throws<InvalidOperationException>(() =>
            votingSystem.AddDecision(invalidDecision));
    }






    [Theory]
    [MemberData(nameof(TestData.DecisionControlType), MemberType = typeof(TestData))]
    public void Error_In_Decisions(List<Decision> decisions, EVotingSystems type, bool singleDecision)
    {
        var options = new List<Option>
        {
            new Option(1, "Alice"),
            new Option(2, "Bob"),
            new Option(3, "Charlie")
        };

        Assert.Throws<ArgumentException>(() => new DecisionControl().Control(decisions, type, options, singleDecision));
    }






    [Fact]
    public void Create_Vote_From_Already_Existing_Rounds()
    {
        EVotingSystems votingType = EVotingSystems.Plural;
        List<Option> optionsR1 = new List<Option>
        {
            new Option(1, "Alice"),
            new Option(2, "Bob"),
            new Option(3, "Charlie"),
            new Option(4, "David"),
            new Option(5, "Ethan"),
            new Option(6, "Fanny"),
            new Option(7, "Greg")
        };

        List<Option> optionsR2 = new List<Option>
        {
            new Option(1, "Alice"),
            new Option(5, "Ethan"),
            new Option(5, "Fanny"),
            new Option(7, "Greg")
        };

        List<Option> optionsR3 = new List<Option>
        {
            new Option(1, "Alice"),
            new Option(5, "Fanny"),
        };

        int nbRounds = 3;
        int[] qualifiedPerRound = { 3, 2, 1 };
        EVictorySettings victorySettings = EVictorySettings.Relative_Majority;
        bool runAgainIfDraw = false;
        IVictoryStrategy victoryStrategy = new RelativeMajorityStrategy();

        List<Round> rounds = new List<Round>
        {
            new Round(optionsR1, victoryStrategy),
            new Round(optionsR2, victoryStrategy),
            new Round(optionsR3, victoryStrategy),
        };


        var vote = new VotingSystemBase(votingType, optionsR1, nbRounds, qualifiedPerRound, victorySettings, runAgainIfDraw, rounds);


        List<Decision> roundDecisions =
            [
                new Decision(1, 1),
                new Decision(5, 1),
                new Decision(1, 1),
                new Decision(1, 1),
                new Decision(5, 1),
                new Decision(1, 1),
            ];

        vote.AddDecision(roundDecisions, vote.currentRound);


        Assert.Equal(EResult.Winner, vote.GetRoundResult(vote.currentRound));
    }




    [Fact]
    public void Create_Vote_From_Already_Existing_Rounds2()
    {
        EVotingSystems votingType = EVotingSystems.Plural;
        List<Option> optionsR1 = new List<Option>
        {
            new Option(1, "Alice"),
            new Option(2, "Bob"),
            new Option(3, "Charlie"),
            new Option(4, "David"),
            new Option(5, "Ethan"),
            new Option(6, "Fanny"),
            new Option(7, "Greg")
        };

        List<Option> optionsR2 = new List<Option>
        {
            new Option(1, "Alice"),
            new Option(5, "Ethan"),
            new Option(5, "Fanny"),
            new Option(7, "Greg")
        };


        int nbRounds = 3;
        int[] qualifiedPerRound = { 3, 2, 1 };
        EVictorySettings victorySettings = EVictorySettings.Relative_Majority;
        bool runAgainIfDraw = false;
        IVictoryStrategy victoryStrategy = new RelativeMajorityStrategy();

        List<Round> rounds = new List<Round>
        {
            new Round(optionsR1, victoryStrategy),
            new Round(optionsR2, victoryStrategy),
        };


        var vote = new VotingSystemBase(votingType, optionsR1, nbRounds, qualifiedPerRound, victorySettings, runAgainIfDraw, rounds);

        List<Decision> roundDecisions =
            [
                new Decision(1, 1),
                new Decision(5, 1),
                new Decision(1, 1),
                new Decision(1, 1),
                new Decision(5, 1),
                new Decision(1, 1),
            ];

        vote.AddDecision(roundDecisions, vote.currentRound);

        vote.NextRound();

        Assert.Equal(EResult.Inconclusive, vote.GetRoundResult(vote.currentRound));
    }
}
