namespace Domain
{
    public class VoteOption
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Score;

        public VoteOption(string name, string description, int id)
        {
            Id = id;
            Name = name;
            Description = description;
            Score = 0;
        }

    }
}
