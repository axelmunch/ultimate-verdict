using Domain;
using Database;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace TEST.UseCases;

public class UseCaseCreateVote
{
    public ICollection<Database.Round> CalculateRounds(DateTime startDate, TimeSpan roundDuration)
    {
        var roundStartTime = startDate;
        var roundEndTime = roundStartTime.Add(roundDuration);

        var rounds = new List<Database.Round>();

        rounds.Add(new Database.Round
        {
            Name = "Round 1",
            StartTime = new DateTimeOffset(roundStartTime).ToUnixTimeMilliseconds(),
            EndTime = new DateTimeOffset(roundEndTime).ToUnixTimeMilliseconds(),
        });

        return rounds;

    }

    public static List<Domain.Option> GetOptionsByVote(int voteId)
    {
        using (var context = new DatabaseContext())
        {
            var options = context.Options
                .Where(o => o.VoteId == voteId)
                .Select(o => new Domain.Option(o.Id, o.Name))
                .ToList();
            return options;
        }
    }

    public static IVictoryStrategy DetermineVictoryStrategy(VoteData data)
    {
        switch (data.victoryCondition)
        {
            case "absolute majority":
                return new AbsoluteMajorityStrategy();

            case "majority":
                return new RelativeMajorityStrategy();

            case "2/3 majority":
                return new TwoThirdsMajorityStrategy();

            case "last man standing":
                return new BRVictoryStrategy();

            default:
                return new NoVictoryStrategy();
        }
    }

    public static Domain.EVotingSystems DetermineVotingSystem(VoteData data)
    {
        switch (data.type)
        {
            case "plural":
                return EVotingSystems.Plural;

            case "ranked":
                return EVotingSystems.Ranked;

            case "weighted":
                return EVotingSystems.Weighted;

            case "elo":
                return EVotingSystems.ELO;

            default:
                return EVotingSystems.Plural;
        }
    }

    public static EVictorySettings DetermineVictorySettings(VoteData data)
    {
        switch (data.victoryCondition)
        {
            case "absolute majority":
                return EVictorySettings.Absolute_Majority;

            case "majority":
                return EVictorySettings.Relative_Majority;

            case "2/3 majority":
                return EVictorySettings.TwoThirds_Majority;

            case "last man standing":
                return EVictorySettings.LastManStanding;

            default:
                return EVictorySettings.None;
        }
    }

}

public class VoteRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Visibility { get; set; }
    public required string Type { get; set; }
    public int NbRounds { get; set; }
    public required List<int> WinnersByRound { get; set; }
    public required string VictoryCondition { get; set; }
    public bool ReplayOnDraw { get; set; }
    public required List<string> Options { get; set; }
    public long StartDate { get; set; }
    public long RoundDuration { get; set; }
}

public class VoteData
{
    public required int id { get; set; }
    public required string name { get; set; }
    public required string description { get; set; }
    public required string visibility { get; set; }
    public required string type { get; set; }
    public required int nbRounds { get; set; }
    public required int[] winnersByRound { get; set; }
    public required string victoryCondition { get; set; }
    public required bool replayOnDraw { get; set; }
    public required object rounds { get; set; }
    public required object options { get; set; }
    public required object result { get; set; }
}



