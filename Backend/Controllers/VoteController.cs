using Domain;
using Database;

using Microsoft.AspNetCore.Mvc;

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

    [HttpGet(Name = "GetVote")]
    public List<Database.Vote> Get()
    {
        var context = new DatabaseContext();

        var vote = new Vote
        {
            Name = "TestCreate",
            Description = "Ceci est un vote de test pour la création",
            LiveResults = true,
            Visibility = "public",
            Type = "plural",
            NbRounds = 1,
            VictoryCondition = "majority",
            ReplayOnDraw = false
        };

        context.Votes.Add(vote);
        context.SaveChanges();

        return context.Votes.ToList();
    }
}

