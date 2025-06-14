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

    public EResult GetResult()
    {
        return VictoryStrategy.CheckResult(Options);
    }
}
