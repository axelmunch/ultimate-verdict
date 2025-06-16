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

    [HttpGet("GetVote", Name = "GetVote")]
    public List<Database.Vote> Get()
    {
        var context = new DatabaseContext();

        return context.Votes.ToList();
    }

    [HttpGet("{id}", Name = "GetVoteById")]
    public ActionResult<Database.Vote> GetVoteById(int id)
    {
        using (var context = new DatabaseContext())
        {
            var vote = context.Votes.FirstOrDefault(v => v.Id == id);

            if (vote == null)
            {
                return NotFound($"Aucun vote trouvé avec l'ID {id}");
            }

            return Ok(vote);
        }
    }

    [HttpPost("CreateVote", Name = "CreateVote")]
    public ActionResult<int> CreateVote([FromBody] Vote vote)
    {
        using (var context = new DatabaseContext())
        {
            context.Votes.Add(vote);
            context.SaveChanges();

            return Ok(vote.Id);
        }
    }
}
