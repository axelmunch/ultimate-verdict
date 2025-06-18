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

    private TEST.UseCases.VoteData? GetVoteDetailsById(int voteId, DatabaseContext context)
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
            .FromSqlRaw("SELECT * FROM \"Rounds\" WHERE \"idVote\" = {0}", voteId)
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

        return new TEST.UseCases.VoteData
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
    public ActionResult ParseVoteData([FromBody] TEST.UseCases.VoteRequest voteData)
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


    private void Routine(int voteId, DatabaseContext context)
    {
        TEST.UseCases.VoteData? voteData = GetVoteDetailsById(voteId, context);
        List<Domain.Option> voteOptions = TEST.UseCases.UseCaseCreateVote.GetOptionsByVote(voteId);
        if (voteData == null)
        {
            throw new ArgumentNullException(nameof(voteData), "Vote data cannot be null.");
        }
        EVotingSystems votingSystem = TEST.UseCases.UseCaseCreateVote.DetermineVotingSystem(voteData);
        IVictoryStrategy victoryStrategy = TEST.UseCases.UseCaseCreateVote.DetermineVictoryStrategy(voteData);
        EVictorySettings victorySettings = TEST.UseCases.UseCaseCreateVote.DetermineVictorySettings(voteData);

        //A passrr les rounds
        var vote = new Domain.VotingSystemBase(votingSystem, voteOptions, voteData.nbRounds, voteData.winnersByRound, victorySettings, voteData.replayOnDraw, new List<Domain.Round>());

        //avoir la liste des decision pour le rounds en cours
        //vote.AddDecision(roundDecisions, vote.currentRound);

        var Currentround = context.Rounds
                .Where(r => r.idVote == voteId)
                .OrderBy(r => r.StartTime)
                .Select(r => new
                {
                    StartTime = r.StartTime,
                    EndTime = r.EndTime
                })
                .FirstOrDefault();

        bool dateDepassee = false;

        if (Currentround != null)
        {
            dateDepassee = Currentround.EndTime < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        if (dateDepassee)
        {

            var currentRoundId = context.Rounds
                .Where(r => r.idVote == voteId)
                .OrderBy(r => r.StartTime)
                .Select(r => r.Id)
                .FirstOrDefault();


            List<Database.Decision> roundDecisions = context.Decisions
                .FromSqlRaw("SELECT * FROM \"Decisions\" WHERE \"RoundId\" = {0}", currentRoundId)
                .Include(d => d.RoundOption)
                .ToList();

            List<Domain.Decision> domainDecisions = new List<Domain.Decision>();
            foreach (var decision in roundDecisions)
            {
                if (decision.RoundOption != null)
                {
                    domainDecisions.Add(new Domain.Decision(decision.RoundOption.OptionId, decision.Score));

                }
                else
                {
                    throw new InvalidOperationException($"Decision with ID {decision.Id} has a null RoundOption.");
                }
                vote.AddDecision(domainDecisions, vote.currentRound);


                if (vote.NextRound())
                {
                    long roundDuration = 0;

                    if (Currentround != null)
                    {
                        roundDuration = Currentround.EndTime - Currentround.StartTime;

                        var newRound = new Database.Round
                        {
                            Name = $"Round {vote.currentRound}",
                            StartTime = Currentround.EndTime,
                            EndTime = Currentround.EndTime + roundDuration,
                            idVote = voteId,
                        };

                        context.Rounds.Add(newRound);
                        context.SaveChanges();

                        int value = newRound.Id; ;

                        List<Domain.Option> newOptions = vote.Rounds[vote.currentRound - 1].Options;

                        foreach (var option in newOptions)
                        {
                            context.RoundOptions.Add(new RoundOption
                            {
                                OptionId = option.Id,
                                RoundId = value
                            });
                        }
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException("Current round data is null.");
                    }
                }
                else
                {
                    //calculte the winner
                    var winners = vote.GetVoteWinner();
                }
            }
        }
    }

}
