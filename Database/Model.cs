using Microsoft.EntityFrameworkCore;

namespace Database;

public class DatabaseContext : DbContext
{
    public DbSet<Option> Options { get; set; }
    public DbSet<Result> Results { get; set; }
    public DbSet<Decision> Decisions { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<Round> Rounds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = "Host=localhost;Port=5432;Database=db;Username=user;Password=password";
        options.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Option>(entity =>
        {
            entity.Property(o => o.Id).IsRequired();
            entity.ToTable("Option");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.Property(r => r.Res)
                .IsRequired()
                .HasMaxLength(12);

            entity.Property(r => r.Id).IsRequired();

            entity.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Results_Res", "res IN ('winner', 'draw', 'inconclusive')");
            });
        });

        modelBuilder.Entity<Decision>(entity =>
        {
            entity.Property(d => d.Id).IsRequired();
            entity.ToTable("Decision");
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.Property(v => v.Visibility).IsRequired();
            entity.Property(v => v.Type).IsRequired();
            entity.Property(v => v.VictoryCondition).IsRequired();

            entity.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Votes_Visibility", "visibility IN ('public', 'private')");
                t.HasCheckConstraint("CK_Votes_Type", "type IN ('plural', 'ranked', 'weighted', 'elo')");
                t.HasCheckConstraint("CK_Votes_VictoryCondition", "victory_condition IN ('none', 'majority', 'absolute majority', '2/3 majority', 'last man standing')");
            });
        });

        modelBuilder.Entity<Round>(entity =>
        {
            entity.Property(r => r.Id).IsRequired();
        });
    }
}
public class Option
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Result
{
    public int Id { get; set; }
    public string Res { get; set; }
}

public class Decision
{
    public int Id { get; set; }
    public int Score { get; set; }
}

public class Vote
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool LiveResults { get; set; } = false;
    public string Visibility { get; set; }
    public string Type { get; set; }
    public int NbRounds { get; set; }
    public string VictoryCondition { get; set; }
    public bool ReplayOnDraw { get; set; } = false;

    public int? ResultId { get; set; }
    public Result? Result { get; set; }

    public ICollection<Round> Rounds { get; set; }
}

public class Round
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long StartTime { get; set; }
    public long EndTime { get; set; }
    public int VoteId { get; set; }
    public Vote Vote { get; set; }

}
