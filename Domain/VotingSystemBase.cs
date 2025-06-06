namespace Domain
{
    public abstract class VotingSystemBase : IVotingSystem
    {
        public abstract string Name { get; }
        private List<Candidate> choiceTable = new();

        public string winnerName = "No winner";

        public VotingSystemBase(List<Candidate> candidates)
        {
            foreach (Candidate candidate in candidates)
            {
                AddCandidate(candidate.Name, candidate.Description);
            }
        }

        public virtual void AddCandidate(string candidateName, string candidateDescription)
        {
            choiceTable.Add(new(candidateName, candidateDescription));
        }

        public virtual void AddVote(string canditateName, int scoreToAdd)
        {
            var entry = choiceTable.FirstOrDefault(cs => cs.Name == canditateName);

            if (entry != null)
            {
                entry.Score += scoreToAdd;
            }
            else
            {
                Console.WriteLine($"Candidat '{canditateName}' non trouvé.");
            }
        }

        public EResult GetResult()
        {
            if (choiceTable.Count == 0)
                return EResult.Inconclusive;

            int maxScore = choiceTable.Max(cs => cs.Score);
            int countMax = choiceTable.Count(cs => cs.Score == maxScore);

            if (maxScore == 0)
                return EResult.Inconclusive;

            if (countMax > 1)
                return EResult.Draw;

            if (countMax == 1)
            {
                winnerName = choiceTable.First(cs => cs.Score == maxScore).Name;
                return EResult.Winner;
            }
            
            return EResult.Inconclusive;
        }
    }
}