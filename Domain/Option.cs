namespace Domain
{
    public class Option
    {
        public int Id { get; }
        public string Name { get; set; }
        public int Score;

        public Option(int id, string name)
        {
            Id = id;
            Name = name;
            Score = 0;
        }
    }
}
