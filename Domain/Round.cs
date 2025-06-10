using Domain;

public class Round
{
    public List<Option> Options;
    private IVictoryStrategy VictoryStrategy;
    private Result result;

    public Round(List<Option> options, IVictoryStrategy victoryStrategy)
    {
        Options = options;
        VictoryStrategy = victoryStrategy;
        result = new Result(options);
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
        return VictoryStrategy.CheckResult(Options);
    }
}
