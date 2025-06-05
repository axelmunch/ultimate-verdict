namespace Domain
{
    public abstract class VotingSystemBase : IVotingSystem
    {
        public abstract string Name { get; }
        private Dictionary<Candidate, int> scores = new();

        public VotingSystemBase(List<Candidate> candidates)
        {
            foreach(Candidate candidate in candidates)
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

        public void GetResult()
        {

        }
    }
}