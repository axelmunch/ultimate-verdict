using Microsoft.EntityFrameworkCore;

namespace Database;

public class DatabaseContext : DbContext
{
    public DbSet<Option> Options { get; set; }

    public DbSet<RoundOption> RoundOptions { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<Round> Rounds { get; set; }

    public DbSet<Decision> Decisions { get; set; }

    public DatabaseContext()
    {
        Database.Migrate();
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            DotNetEnv.Env.Load();

            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = "5432";
            var database = Environment.GetEnvironmentVariable("DB_DB") ?? "db";
            var username = Environment.GetEnvironmentVariable("DB_USER") ?? "user";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";

            var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

            options.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Option>(entity =>
        {
            entity.Property(o => o.Id).IsRequired();
            entity.Property(o => o.Name).IsRequired();

            entity.HasOne(o => o.Vote)
                .WithMany(v => v.Options)
                .HasForeignKey(o => o.VoteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.ToTable("Option");
        });

        modelBuilder.Entity<Decision>(entity =>
        {
            entity.ToTable("Decisions");
            entity.HasKey(d => d.Id);

            entity.HasOne(d => d.RoundOption)
                .WithMany()
                .HasForeignKey(d => new { d.OptionId, d.RoundId })
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(d => d.Score).IsRequired();
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

        modelBuilder.Entity<Round>()
            .HasOne(r => r.Vote)
            .WithMany(v => v.Rounds)
            .HasForeignKey(r => r.idVote)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoundOption>(entity =>
        {
            entity.HasKey(ro => new { ro.OptionId, ro.RoundId });
            entity.ToTable("RoundOptions");

            entity.HasOne(ro => ro.Option)
                .WithMany()
                .HasForeignKey(ro => ro.OptionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ro => ro.Round)
                .WithMany()
                .HasForeignKey(ro => ro.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

public class Option
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public required int VoteId { get; set; }
    public Vote? Vote { get; set; }
}

public class Decision
{
    public int Id { get; set; }
    public int OptionId { get; set; }
    public int RoundId { get; set; }

    public RoundOption? RoundOption { get; set; }

    public int Score { get; set; }
}
public class Vote
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Visibility { get; set; }
    public required string Type { get; set; }
    public required int NbRounds { get; set; }
    public required ICollection<int> WinnersByRound { get; set; }
    public required string VictoryCondition { get; set; }
    public required bool ReplayOnDraw { get; set; }

    public required ICollection<Round> Rounds { get; set; }

    public required ICollection<Option> Options { get; set; }
}

public class Round
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required long StartTime { get; set; }
    public required long EndTime { get; set; }

    public int idVote { get; set; }
    public Vote? Vote { get; set; }
}

public class RoundOption
{
    public required int OptionId { get; set; }
    public Option? Option { get; set; }

    public required int RoundId { get; set; }
    public Round? Round { get; set; }
}
