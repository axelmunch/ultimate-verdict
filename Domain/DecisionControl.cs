using Domain;

public class DecisionControl
{
    private IVerifyVotetrategy _verifyVoteStrategy = new PluralVoteStrategy();
    public void Control(List<Decision> decisions, EVotingSystems type, List<Option> options)
    {
        foreach (var decision in decisions)
        {
            if (!options.Any(o => o.Id == decision.Id))
            {
                throw new InvalidOperationException($"Candidate with id {decision.Id} does not exist in current options.");
            }

            SetVoteSystemStrategy(type);

            _verifyVoteStrategy.CheckVote(decisions);
        }
    }

    private void SetVoteSystemStrategy(EVotingSystems type)
    {
        switch (type)
        {
            case EVotingSystems.Plural:
                _verifyVoteStrategy = new PluralVoteStrategy();
                break;
            case EVotingSystems.Weighted:
                _verifyVoteStrategy = new WeightedVoteStrategy();
                break;
            case EVotingSystems.Ranked:
                _verifyVoteStrategy = new RankedVoteStrategy();
                break;
            case EVotingSystems.ELO:
                _verifyVoteStrategy = new ELOVoteStrategy();
                break;
        }
    }
}
