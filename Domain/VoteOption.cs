namespace Domain
{
    public class VoteOption
    {
        public int Id { get; }
        public string Name { get; set; }
        public int Score;

        public VoteOption(string name, int id)
        {
            Id = id;
            Name = name;
            Score = 0;
        }

    }
}
