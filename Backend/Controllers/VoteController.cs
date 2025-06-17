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

    private object? GetVoteDetailsById(int voteId, DatabaseContext context)
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
            .FromSqlRaw("SELECT * FROM \"Rounds\" WHERE \"VoteId\" = {0}", voteId)
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
                    .ToList()
            })
            .ToList();

        var vote = context.Votes.FirstOrDefault(v => v.Id == voteId);

        if (vote == null)
        {
            return null;
        }

        return new
        {
            id = voteId,
            name = vote.Name,
            description = vote.Description,
            visibility = vote.Visibility,
            type = vote.Type,
            nbRounds = vote.NbRounds,
            winnersByRounds = vote.WinnersByRounds,
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
                WinnersByRounds = voteData.WinnersByRound,
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

            return Ok(vote.Id);

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
