public interface IVotingSystem
{
    string Name { get; }
    void AddVote();
    void GetStanding();
    void GetResult();
}