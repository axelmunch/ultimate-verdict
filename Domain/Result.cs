namespace Domain
{
    public class Result(List<VoteOption> voteOptions)
    {
        private List<VoteOption> Options = voteOptions;
        private List<int> scores = []; //Meme taille que options
        public EResult result;
    }
}