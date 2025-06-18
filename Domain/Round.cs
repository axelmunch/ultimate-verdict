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
            throw new InvalidOperationException($"Candidate '{id}' not found.");
    }

    public EResult GetResult()
    {
        result.Options = VictoryStrategy.GetRoundStanding(Options);
        result.result = VictoryStrategy.CheckResult(Options);
        return result.result;
    }

    public List<Option> DetermineQualified(int nbQualified)
    {
        var standing = GetStanding();

        if (nbQualified > standing.Count)
            throw new InvalidOperationException($"Impossible de qualifier {nbQualified} candidats alors qu’il n’en reste que {standing.Count}.");

        int minQualifiedScore = standing.Take(nbQualified).Last().Score;

        return standing
            .Where(vo => vo.Score >= minQualifiedScore)
            .Select(vo => new Option(vo.Id, vo.Name))
            .ToList();
    }

    private List<Option> GetStanding()
    {
        return Options
            .OrderByDescending(vo => vo.Score)
            .ToList();
    }
}
