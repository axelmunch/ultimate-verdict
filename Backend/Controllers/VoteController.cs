using Domain;
using Database;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;


namespace TEST.Controllers;

[ApiController]
[Route("[controller]")]
public class VoteController : ControllerBase
{
    private readonly ILogger<VoteController> _logger;

    public VoteController(ILogger<VoteController> logger)
    {
        _logger = logger;
    }

    private VoteData? GetVoteDetailsById(int voteId, DatabaseContext context)
    {
        var options = context.Options
            .Where(o => o.VoteId == voteId)
            .Select(o => new
            {
                id = o.Id,
                name = o.Name
            })
            .ToList();

        var rounds = context.Rounds
            .Where(r => r.idVote == voteId)
            .Select(r => new
            {
                id = r.Id,
                name = r.Name,
                startTime = r.StartTime,
                endTime = r.EndTime,
                options = context.RoundOptions
                    .Where(ro => ro.RoundId == r.Id)
                    .Select(ro => new
                    {
                        id = ro.OptionId,
                        name = context.Options
                            .Where(o => o.Id == ro.OptionId)
                            .Select(o => o.Name)
                            .FirstOrDefault()
                    })
                    .ToList(),
                result = (object?)null
            })
            .ToList();

        var vote = context.Votes.FirstOrDefault(v => v.Id == voteId);

        if (vote == null)
        {
            return null;
        }

        return new VoteData
        {
            id = voteId,
            name = vote.Name,
            description = vote.Description,
            visibility = vote.Visibility,
            type = vote.Type,
            nbRounds = vote.NbRounds,
            winnersByRound = vote.WinnersByRound.ToArray(),
            victoryCondition = vote.VictoryCondition,
            replayOnDraw = vote.ReplayOnDraw,
            rounds = rounds,
            options = options,
            result = (object?)null
        };
    }

    [HttpGet("GetVote", Name = "GetVote")]
    public ActionResult GetAllVotes()
    {
        using (var context = new DatabaseContext())
        {
            var voteIds = context.Votes.Select(v => v.Id).ToList();

            var allVotes = voteIds
                .Select(voteId => GetVoteDetailsById(voteId, context))
                .ToList();

            return Ok(allVotes);
        }
    }

    [HttpGet("{voteId}", Name = "GetVoteById")]
    public ActionResult GetVoteById(int voteId)
    {
        Routine(voteId, new DatabaseContext());
        using (var context = new DatabaseContext())
        {
            var voteDetails = GetVoteDetailsById(voteId, context);

            if (voteDetails == null)
            {
                return NotFound($"Vote avec l'ID {voteId} introuvable.");
            }

            return Ok(voteDetails);
        }
    }

    [HttpPost("CreateVote", Name = "CreateVote")]
    public ActionResult ParseVoteData([FromBody] VoteRequest voteData)
    {
        try
        {
            Vote vote = new Vote
            {
                Name = voteData.Name,
                Description = voteData.Description,
                Visibility = voteData.Visibility,
                Type = voteData.Type,
                NbRounds = voteData.NbRounds,
                WinnersByRound = voteData.WinnersByRound,
                VictoryCondition = voteData.VictoryCondition,
                ReplayOnDraw = voteData.ReplayOnDraw,
                Options = voteData.Options.Select(o => new Database.Option { Name = o, VoteId = 0 }).ToList(),
                Rounds = (ICollection<Database.Round>)CalculateRounds(
                    DateTimeOffset.FromUnixTimeMilliseconds(voteData.StartDate).UtcDateTime,
                    TimeSpan.FromMilliseconds(voteData.RoundDuration)
                )
            };


            using (var context = new DatabaseContext())
            {
                context.Votes.Add(vote);
                context.SaveChanges();
            }

            using (var context = new DatabaseContext())
            {
                foreach (var round in vote.Rounds)
                {
                    foreach (var option in vote.Options)
                    {
                        context.RoundOptions.Add(new RoundOption
                        {
                            RoundId = round.Id,
                            OptionId = option.Id
                        });
                    }
                }
                context.SaveChanges();
            }

            return StatusCode(201, vote.Id);

        }
        catch (Exception ex)
        {
            return BadRequest($"Erreur lors de l'analyse des données : {ex.Message}");
        }
    }

    private ICollection<Database.Round> CalculateRounds(DateTime startDate, TimeSpan roundDuration)
    {
        var roundStartTime = startDate.Add(roundDuration);
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


    private void Routine(int voteId, DatabaseContext context)
    {
        VoteData voteData = GetVoteDetailsById(voteId, context);
        List<Domain.Option> voteOptions = GetOptionsByVote(voteId);
        EVotingSystems votingSystem = DetermineVotingSystem(voteData);
        IVictoryStrategy victoryStrategy = DetermineVictoryStrategy(voteData);
        EVictorySettings victorySettings = DetermineVictorySettings(voteData);

        //A passrr les rounds
        var vote = new Domain.VotingSystemBase(votingSystem, voteOptions, voteData.nbRounds, voteData.winnersByRound, victorySettings, voteData.replayOnDraw, new List<Domain.Round>());

        //avoir la liste des decision pour le rounds en cours
        //vote.AddDecision(roundDecisions, vote.currentRound);

        bool dateDepassee = false;
        if (vote.NextRound() && dateDepassee)
        {
            var round = context.Rounds
                .Where(r => r.idVote == voteId)
                .OrderBy(r => r.StartTime)
                .Select(r => new
                {
                    StartTime = r.StartTime,
                    EndTime = r.EndTime
                })
                .FirstOrDefault();

            if (round == null)
            {
                throw new Exception($"Aucun round trouvé pour le vote avec l'ID {voteId}.");
            }

            var roundDuration = round.EndTime - round.StartTime;

            var newRound = new Database.Round
            {
                Name = $"Round {vote.currentRound + 1}",
                StartTime = round.StartTime + roundDuration,
                EndTime = round.EndTime + roundDuration,
                idVote = voteId
            };

            context.Rounds.Add(newRound);
            context.SaveChanges();

        }

    }

    private List<Domain.Option> GetOptionsByVote(int voteId)
    {
        using (var context = new DatabaseContext())
        {
            var options = context.Options
                .Where(o => o.VoteId == voteId)
                .Select(o => new Domain.Option(o.Id, o.Name))
                .ToList();

            foreach (var option in options)
            {
                Console.WriteLine(option.Id);
                Console.WriteLine(option.Name);
            }

            return options;
        }
    }

    private IVictoryStrategy DetermineVictoryStrategy(VoteData data)
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

    private Domain.EVotingSystems DetermineVotingSystem(VoteData data)
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

    private EVictorySettings DetermineVictorySettings(VoteData data)
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



