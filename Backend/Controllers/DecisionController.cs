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
    public IActionResult CreateDecision([FromBody] TEST.UseCases.DecisionRequest decisionData)
    {
        try
        {
            TEST.UseCases.UseCaseCreateDecision.Validate(decisionData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating decision");
            return StatusCode(500, "Internal server error");
        }
        return StatusCode(201, 0);

    }
}


