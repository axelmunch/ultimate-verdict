using Database;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests;

public class UnitTest1
{
    private DbContextOptions<DatabaseContext> GetInMemoryDbContextOptions()
    {
        return new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void CreateVote()
    {
        var options = GetInMemoryDbContextOptions();
        using (var context = new DatabaseContext(options))
        {
            var vote = new Vote
            {
                Name = "TestCreate",
                Description = "Ceci est un vote de test pour la création",
                LiveResults = true,
                Visibility = "public",
                Type = "plural",
                NbRounds = 1,
                VictoryCondition = "majority",
                ReplayOnDraw = false
            };

            context.Votes.Add(vote);
            context.SaveChanges();

            var insertedVote = context.Votes.FirstOrDefault(v => v.Name == "TestCreate");
            Assert.NotNull(insertedVote);
            Assert.Equal("TestCreate", insertedVote.Name);
        }
    }
    [Fact]
    public void CreateVoteWithError()
    {
        var options = GetInMemoryDbContextOptions();
        using (var context = new DatabaseContext())
        {
            var vote = new Vote
            {
                Name = "TestCreate",
                Description = "Ceci est un vote de test pour la création",
                LiveResults = true,
                Visibility = "public",
                Type = "plural",
                NbRounds = 1,
                VictoryCondition = "false value",
                ReplayOnDraw = false
            };


            Assert.Throws<DbUpdateException>(() =>
            {
                context.Votes.Add(vote);
                context.SaveChanges();
            });
        }
    }

    [Fact]
    public void ReadVote()
    {
        var options = GetInMemoryDbContextOptions();
        using (var context = new DatabaseContext(options))
        {
            var vote = new Vote
            {
                Name = "TestRead",
                Description = "Ceci est un vote de test pour la lecture",
                LiveResults = true,
                Visibility = "public",
                Type = "plural",
                NbRounds = 1,
                VictoryCondition = "majority",
                ReplayOnDraw = false
            };
            context.Votes.Add(vote);
            context.SaveChanges();

            var retrievedVote = context.Votes.FirstOrDefault(v => v.Name == "TestRead");

            Assert.NotNull(retrievedVote);
            Assert.Equal("TestRead", retrievedVote.Name);
        }
    }

    [Fact]
    public void UpdateVote()
    {
        var options = GetInMemoryDbContextOptions();
        using (var context = new DatabaseContext(options))
        {
            var vote = new Vote
            {
                Name = "TestUpdate",
                Description = "Ceci est un vote de test pour la mise à jour",
                LiveResults = true,
                Visibility = "public",
                Type = "plural",
                NbRounds = 1,
                VictoryCondition = "majority",
                ReplayOnDraw = false
            };
            context.Votes.Add(vote);
            context.SaveChanges();
        }

        using (var context = new DatabaseContext(options))
        {
            var voteToUpdate = context.Votes.FirstOrDefault(v => v.Name == "TestUpdate");
            Assert.NotNull(voteToUpdate);
            voteToUpdate.Description = "Description mise à jour";
            context.SaveChanges();
        }

        using (var context = new DatabaseContext(options))
        {
            var updatedVote = context.Votes.FirstOrDefault(v => v.Name == "TestUpdate");
            Assert.NotNull(updatedVote);
            Assert.Equal("Description mise à jour", updatedVote.Description);
        }
    }

    [Fact]
    public void DeleteVote()
    {
        var options = GetInMemoryDbContextOptions();
        using (var context = new DatabaseContext(options))
        {
            var vote = new Vote
            {
                Name = "TestDelete",
                Description = "Ceci est un vote de test pour la suppression",
                LiveResults = true,
                Visibility = "public",
                Type = "plural",
                NbRounds = 1,
                VictoryCondition = "majority",
                ReplayOnDraw = false
            };
            context.Votes.Add(vote);
            context.SaveChanges();
        }

        using (var context = new DatabaseContext(options))
        {
            var voteToDelete = context.Votes.FirstOrDefault(v => v.Name == "TestDelete");
            Assert.NotNull(voteToDelete);
            context.Votes.Remove(voteToDelete);
            context.SaveChanges();
        }

        using (var context = new DatabaseContext(options))
        {
            var deletedVote = context.Votes.FirstOrDefault(v => v.Name == "TestDelete");
            Assert.Null(deletedVote);
        }
    }
}