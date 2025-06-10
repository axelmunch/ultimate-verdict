namespace Domain
{
    public class VoteOption
    {
        public int Id { get; }
        public string Name { get; set; }
        public int Score;

        public VoteOption(int id, string name)
        {
            Id = id;
            Name = name;
            Score = 0;
        }
    }
}
