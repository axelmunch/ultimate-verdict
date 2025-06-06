namespace Domain
{
    public class VoteOption
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Score;

        public VoteOption(string name, string description)
        {
            Name = name;
            Description = description;
            Score = 0;
        }

    }
}