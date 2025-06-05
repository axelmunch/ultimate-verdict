namespace Domain
{
    public abstract class VotingSystemBase : IVotingSystem
    {
        public abstract string Name { get; }
        private Dictionary<Candidate, int> scores = new();

        public VotingSystemBase(List<Candidate> candidates)
        {
            foreach (Candidate candidate in candidates)
            {
                AddCandidate(candidate.Name, candidate.Description);
            }
        }

        public virtual void AddCandidate(string candidateName, string candidateDescription)
        {
            Candidate candidateToAdd = new(candidateName, candidateDescription);
            scores.Add(candidateToAdd, 0);
        }

        public virtual void AddVote(string canditateName, int scoreToAdd)
        {
            var candidateIndex = scores.Keys.FirstOrDefault(c => c.Name == canditateName);

            if (candidateIndex != null)
            {
                scores[candidateIndex]++;
            }
            else
            {
                Console.WriteLine($"Candidat '{canditateName}' non trouvé.");
            }
        }

        public void GetStanding()
        {

        }

        public EResult GetResult()
        {
            if (scores.Count == 0)
                return EResult.Inconclusive;

            int maxScore = scores.Values.Max();
            int countMax = scores.Values.Count(v => v == maxScore);

            if (countMax == 1)
                return EResult.Winner;
            else if (countMax > 1)
                return EResult.Draw;
            else
                return EResult.Inconclusive;
        }

        public string GetWinner()
        {
            if (scores.Count == 0)
                return "Error: No candidates";

            int maxScore = scores.Values.Max();
            var gagnant = scores.FirstOrDefault(kv => kv.Value == maxScore).Key;

            return gagnant?.Name ?? "No winner";
        }
    }
}