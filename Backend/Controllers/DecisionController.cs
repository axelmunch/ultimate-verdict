using Domain;
using Database;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication.PgOutput.Messages;


namespace TEST.Controllers;

[ApiController]
[Route("[controller]")]
public class DecisionController : ControllerBase
{
    private readonly ILogger<DecisionController> _logger;

    public DecisionController(ILogger<DecisionController> logger)
    {
        _logger = logger;
    }

    [HttpPost("CreateDecision", Name = "CreateDecision")]
    public IActionResult CreateDecision([FromBody] DecisionRequest decisionData)
    {
        try
        {
            ValidDataInDomain(decisionData);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid data Send");
            return BadRequest("Le controlleur t'as mangé");
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error validating decision data");
            return StatusCode(500, "Internal server error");
        }

        try
        {
            var roundId = decisionData.RoundId;
            var decisions = decisionData.Decisions;

            foreach (var decision in decisions)
            {
                using (var context = new DatabaseContext())
                {
                    var roundOption = context.RoundOptions
                        .FirstOrDefault(ro => ro.OptionId == decision.Id && ro.RoundId == roundId);

                    if (roundOption == null)
                    {
                        return BadRequest($"RoundOption introuvable pour OptionId={decision.Id} et RoundId={roundId}.");
                    }

                    var newDecision = new Database.Decision
                    {
                        RoundOption = roundOption,
                        Score = decision.Score
                    };

                    Domain.Decision domainDecision = new Domain.Decision(newDecision.Id, newDecision.Score);


                    context.Decisions.Add(newDecision);
                    context.SaveChanges();
                }
            }
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating decision");
            return StatusCode(500, "Internal server error");
        }
        return StatusCode(201, "Decision created successfully");

    }

    private void ValidDataInDomain(DecisionRequest decisionData)
    {
        List<Domain.Decision> decisions = new List<Domain.Decision>();
        var type = EVotingSystems.Plural;
        var options = new List<Domain.Option>();
        bool singleDecision = true;

        foreach (var decision in decisionData.Decisions)
        {
            decisions.Add(new Domain.Decision(decision.Id, decision.Score));
        }

        var idOfTheRound = decisionData.RoundId;

        var idOfTheVote = 0;
        using (var context = new DatabaseContext())
        {
            var vote = context.Votes
                .Include(v => v.Rounds)
                .Include(v => v.Options)
                .FirstOrDefault(v => v.Rounds.Any(r => r.Id == idOfTheRound));

            if (vote == null)
            {
                throw new ArgumentException($"Aucun vote trouvé pour le round avec l'ID {idOfTheRound}.");
            }
            if (vote.Options == null)
            {
                throw new ArgumentException($"Les options pour le vote avec l'ID {vote.Id} sont nulles.");
            }

            if (vote != null)
            {
                idOfTheVote = vote.Id;
                type = (EVotingSystems)Enum.Parse(typeof(EVotingSystems), vote.Type, true);
                options = vote.Options.Select(o => new Domain.Option(o.Id, o.Name)).ToList();
            }
        }
        new DecisionControl().Control(decisions, type, options, singleDecision);
    }

    public class DecisionRequest
    {
        public int RoundId { get; set; }
        public required List<DecisionDetailsRequest> Decisions { get; set; }
    }

    public class DecisionDetailsRequest
    {
        public int Id { get; set; }
        public int Score
        {
            get; set;
        }

    }
}


