using Domain;
using Database;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication.PgOutput.Messages;
using Microsoft.VisualBasic;


namespace TEST.UseCases;

public class UseCaseCreateDecision
{
    public static void Validate(DecisionRequest decisionData)
    {
        try
        {
            ValidDataInDomain(decisionData);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"Invalid data: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException($"Invalid operation: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            throw new DbUpdateException($"Database update error: {ex.Message}");
        }

        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred: {ex.Message}");
        }


        var roundId = decisionData.RoundId;
        var decisions = decisionData.Decisions;

        SaveDecisionsToDatabase(roundId, decisions);
    }

    private static void SaveDecisionsToDatabase(int roundId, List<DecisionDetailsRequest> decisions)
    {
        foreach (var decision in decisions)
        {
            using (var context = new DatabaseContext())
            {
                var roundOption = context.RoundOptions
                    .FirstOrDefault(ro => ro.OptionId == decision.Id && ro.RoundId == roundId);

                if (roundOption == null)
                {
                    throw new ArgumentException($"Aucune option trouvée pour l'ID {decision.Id} dans le round avec l'ID {roundId}.");
                }

                var newDecision = new Database.Decision
                {
                    RoundOption = roundOption,
                    Score = decision.Score
                };

                Domain.Decision domainDecision = new Domain.Decision(newDecision.Id, newDecision.Score);


                context.Decisions.Add(newDecision);
                context.SaveChanges();
            }
        }
    }

    private static void ValidDataInDomain(DecisionRequest decisionData)
    {
        List<Domain.Decision> decisions = new List<Domain.Decision>();
        var type = EVotingSystems.Plural;
        var options = new List<Domain.Option>();
        bool singleDecision = true;

        foreach (var decision in decisionData.Decisions)
        {
            decisions.Add(new Domain.Decision(decision.Id, decision.Score));
        }

        var idOfTheRound = decisionData.RoundId;

        var idOfTheVote = 0;
        using (var context = new DatabaseContext())
        {
            var vote = context.Votes
                .Include(v => v.Rounds)
                .Include(v => v.Options)
                .FirstOrDefault(v => v.Rounds.Any(r => r.Id == idOfTheRound));

            if (vote == null)
            {
                throw new ArgumentException($"Aucun vote trouvé pour le round avec l'ID {idOfTheRound}.");
            }
            if (vote.Options == null)
            {
                throw new ArgumentException($"Les options pour le vote avec l'ID {vote.Id} sont nulles.");
            }

            if (vote != null)
            {
                idOfTheVote = vote.Id;
                type = (EVotingSystems)Enum.Parse(typeof(EVotingSystems), vote.Type, true);
                options = vote.Options.Select(o => new Domain.Option(o.Id, o.Name)).ToList();
            }
        }
        new DecisionControl().Control(decisions, type, options, singleDecision);
    }
}

public class DecisionRequest
{
    public int RoundId { get; set; }
    public required List<DecisionDetailsRequest> Decisions { get; set; }
}

public class DecisionDetailsRequest
{
    public int Id { get; set; }
    public int Score
    {
        get; set;
    }

}


