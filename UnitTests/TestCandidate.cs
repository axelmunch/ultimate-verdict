namespace UnitTests
{

    public class TestCandidate
    {
        public int Id { get; }
        public string Name { get; }
        public string Description => $"Candidat {Id}";
        public List<int> ScoresPerRound { get; }

        public TestCandidate(int id, string name, List<int> scoresPerRound)
        {
            Id = id;
            Name = name;
            ScoresPerRound = scoresPerRound;
        }
    }
}
