using Domain;

public class DecisionControl
{

    private IVerifyVotetrategy _verifyVoteStrategy = new PluralVoteStrategy();
    public void Control(List<Decision> decisions, EVotingSystems type, List<Option> options, bool singleDecision)
    {
        foreach (var decision in decisions)
        {
            if (!options.Any(o => o.Id == decision.Id))
            {
                throw new InvalidOperationException($"Candidate with id {decision.Id} does not exist in current options.");
            }

            _verifyVoteStrategy = VotingStrategyFactory.Create(type);
            _verifyVoteStrategy.CheckVote(decisions, singleDecision);
        }
    }

}
