using Microsoft.EntityFrameworkCore;

namespace Database;

public class DatabaseContext : DbContext
{
    public DbSet<Option> Options { get; set; }

    public DbSet<RoundOption> RoundOptions { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<Round> Rounds { get; set; }

    public DatabaseContext()
    {
        Database.Migrate();
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            var connectionString = "Host=localhost;Port=5432;Database=db;Username=user;Password=password";
            options.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>(entity =>
        {
            entity.Property(o => o.Id).IsRequired();
            entity.ToTable("Option");
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

            entity.ToTable("Votes", t =>
            {
                t.HasCheckConstraint("CK_Votes_Visibility", "\"Visibility\" IN ('public', 'private')");
                t.HasCheckConstraint("CK_Votes_Type", "\"Type\" IN ('plural', 'ranked', 'weighted', 'elo')");
                t.HasCheckConstraint("CK_Votes_VictoryCondition", "\"VictoryCondition\" IN ('none', 'majority', 'absolute majority', '2/3 majority', 'last man standing')");
            });
        });

        modelBuilder.Entity<Round>(entity =>
        {
            entity.Property(r => r.Id).IsRequired();
            entity.ToTable("Rounds");
        });

        modelBuilder.Entity<RoundOption>(entity =>
        {
            entity.HasKey(ro => new { ro.OptionId, ro.RoundId });
            entity.ToTable("RoundOptions");

            entity.HasOne<Option>()
            .WithMany()
            .HasForeignKey(ro => ro.OptionId);

            entity.HasOne<Round>()
            .WithMany()
            .HasForeignKey(ro => ro.RoundId);
        });
    }
}

public class Option
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class Decision
{
    public int Id { get; set; }
    public int Score { get; set; }
}

public class Vote
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool LiveResults { get; set; } = false;
    public required string Visibility { get; set; }
    public required string Type { get; set; }
    public int NbRounds { get; set; }
    public required string VictoryCondition { get; set; }
    public bool ReplayOnDraw { get; set; } = false;

    public int? ResultId { get; set; }

    public required ICollection<Round> Rounds { get; set; }
}

public class Round
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public long StartTime { get; set; }
    public long EndTime { get; set; }
    public int VoteId { get; set; }
    public required Vote Vote { get; set; }
}

public class RoundOption
{
    public int OptionId { get; set; }
    public int RoundId { get; set; }
}
