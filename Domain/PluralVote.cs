namespace Domain
{
    public class PluralVote : VotingSystemBase
    {
        public override string Name => "Plural Vote";

        public PluralVote(List<Candidate> candidates): base(candidates)
        {
        }
    }
}