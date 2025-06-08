using Domain;

public class Round(List<VoteOption> voteOptions, IVictoryStrategy victoryStrategy, IVotinSystemStrategy votinSystemStrategy)
{
    public List<VoteOption> VoteOptions = voteOptions;
    private IVictoryStrategy VictoryStrategy = victoryStrategy;
    private IVotinSystemStrategy VotinSystemStrategy = votinSystemStrategy;
    private Result result = new(voteOptions);


    //TODO: A modifier une fois class result finie    
    public void AddVote(int id, int scoreToAdd)
    {
        var entry = this.VoteOptions.FirstOrDefault(cs => cs.Id == id);

        if (entry != null)
        {
            entry.Score += scoreToAdd;
        }
        else
        {
            Console.WriteLine($"Candidat '{id}' non trouvé.");
        }
    }

    //TODO: A modifier une fois class result finie  
    public EResult GetResult()
    {

        return VictoryStrategy.CheckResult(this);
    }
}