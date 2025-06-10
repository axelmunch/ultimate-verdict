using Domain;

public class Round
{
    public List<VoteOption> VoteOptions;
    private IVictoryStrategy VictoryStrategy;
    private Result result;

    public Round(List<VoteOption> voteOptions, IVictoryStrategy victoryStrategy)
    {
        VoteOptions = voteOptions;
        VictoryStrategy = victoryStrategy;
        result = new Result(voteOptions);
    }


    //TODO: A modifier une fois class result & voteStrat finies
    public void AddVote(int id, int scoreToAdd)
    {
        var entry = result.Options.FirstOrDefault(cs => cs.Id == id);

        if (entry != null)
        {
            entry.Score += scoreToAdd;
        }
        else
            throw new Exception($"Candidate '{id}' not found.");
    }

    //TODO: A modifier une fois class result finie
    public EResult GetResult()
    {
        return VictoryStrategy.CheckResult(VoteOptions);
    }
}
