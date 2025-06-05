namespace UnitTests;

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
        List<Candidate> candidates = new()
        {
            new Candidate(candidate1Name, candidate1Desc),
            new Candidate(candidate2Name, candidate2Desc)
        };

        PluralVote pluralvote = new(candidates);
        pluralvote.AddVote(candidate1Name, candidate1score);
        pluralvote.AddVote(candidate2Name, candidate2score);

        Assert.Equal(expectedResult, pluralvote.GetResult());
        Assert.Equal(expectedWinner, pluralvote.winnerName);
    }
}
