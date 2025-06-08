namespace Domain
{
    public class Result(List<VoteOption> voteOptions)
    {
        public List<VoteOption> Options = voteOptions;
        public EResult result;
    }
}