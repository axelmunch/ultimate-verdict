using Domain;

public class Round(List<VoteOption> voteOptions, IVictoryStrategy victoryStrategy, IVotinSystemStrategy votinSystemStrategy)
{
    public List<VoteOption> VoteOptions = voteOptions;
    private IVictoryStrategy VictoryStrategy = victoryStrategy;
    private IVotinSystemStrategy VotinSystemStrategy = votinSystemStrategy;
    private Result result = new(voteOptions);


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

        return VictoryStrategy.CheckResult(this);
    }
}
