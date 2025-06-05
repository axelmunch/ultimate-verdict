namespace Domain
{
    public class Candidate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Score;

        public Candidate(string name, string description)
        {
            Name = name;
            Description = description;
            Score = 0;
        }

    }
}