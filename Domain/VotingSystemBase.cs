public abstract class VotingSystemBase : IVotingSystem
{
    public abstract string Name { get; }
    private List<Candidate> Candidates = new();

    private Dictionary<Candidate, int> scores;

    public virtual void AddVote()
    {

    }

    public void GetStanding()
    {

    }

    public void GetResult()
    {

    }
}
