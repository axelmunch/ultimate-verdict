using Domain;
using Database;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;


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

        return Ok();
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


