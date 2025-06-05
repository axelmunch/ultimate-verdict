namespace UnitTests;

using Domain;

public class PluralVoteTests
{
    [Fact]
    public void WinnerTest()
    {
        List<Candidate> candidates = new()
        {
            new Candidate("Jean", "Candidat1"),
            new Candidate("Martin", "Candidat2")
        };
        PluralVote pluralvote = new(candidates);
        pluralvote.AddVote("Jean", 1);

        Assert.Equal(EResult.Winner, pluralvote.GetResult());
        Assert.Equal("Jean", pluralvote.GetWinner());
    }
}
