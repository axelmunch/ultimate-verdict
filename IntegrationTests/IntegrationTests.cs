using Database;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests;

public class IntegrationTests
{
    [Fact]
    public void CreateVote()
    {
        using (var context = new DatabaseContext())
        {
            var vote = new Vote
            {
                Name = "TestCreate",
                Description = "Ceci est un vote de test pour la création",
                Visibility = "public",
                Type = "plural",
                NbRounds = 1,
                VictoryCondition = "majority",
                ReplayOnDraw = false,
                WinnersByRound = new List<int>(),
                Rounds = new List<Round>(),
                Options = new List<Option>()
            };

            context.Votes.Add(vote);
            context.SaveChanges();

            var insertedVote = context.Votes.FirstOrDefault(v => v.Name == "TestCreate");
            Assert.NotNull(insertedVote);
            Assert.Equal("TestCreate", insertedVote.Name);
        }
    }

    [Fact]
    public void ReadVote()
    {
        using (var context = new DatabaseContext())
        {
            var vote = new Vote
            {
                Name = "TestRead",
                Description = "Ceci est un vote de test pour la lecture",
                WinnersByRound = new List<int>(),
                Visibility = "public",
                Type = "plural",
                NbRounds = 1,
                VictoryCondition = "majority",
                ReplayOnDraw = false,
                Rounds = new List<Round>(),
                Options = new List<Option>()
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
        using (var context = new DatabaseContext())
        {
            var vote = new Vote
            {
                Name = "TestUpdate",
                Description = "Ceci est un vote de test pour la mise à jour",
                WinnersByRound = new List<int>(),
                Visibility = "public",
                Type = "plural",
                NbRounds = 1,
                VictoryCondition = "majority",
                ReplayOnDraw = false,
                Rounds = new List<Round>(),
                Options = new List<Option>()
            };
            context.Votes.Add(vote);
            context.SaveChanges();

            var voteToUpdate = context.Votes.FirstOrDefault(v => v.Name == "TestUpdate");
            Assert.NotNull(voteToUpdate);
            voteToUpdate.Description = "Description mise à jour";
            context.SaveChanges();

            var updatedVote = context.Votes.FirstOrDefault(v => v.Name == "TestUpdate");
            Assert.NotNull(updatedVote);
            Assert.Equal("Description mise à jour", updatedVote.Description);
        }
    }

    [Fact]
    public void DeleteVote()
    {
        using (var context = new DatabaseContext())
        {
            var vote = new Vote
            {
                Name = "TestDelete",
                Description = "Ceci est un vote de test pour la suppression",
                WinnersByRound = new List<int>(),
                Visibility = "public",
                Type = "plural",
                NbRounds = 1,
                VictoryCondition = "majority",
                ReplayOnDraw = false,
                Rounds = new List<Round>(),
                Options = new List<Option>()
            };
            context.Votes.Add(vote);
            context.SaveChanges();

            var voteToDelete = context.Votes.FirstOrDefault(v => v.Name == "TestDelete");
            Assert.NotNull(voteToDelete);
            context.Votes.Remove(voteToDelete);
            context.SaveChanges();

            var deletedVote = context.Votes.FirstOrDefault(v => v.Name == "TestDelete");
            Assert.Null(deletedVote);
        }
    }

    [Fact]
    public void ConstraintKey_VictoryCondition_ThrowError()
    {
        using (var context = new DatabaseContext())
        {
            var vote = new Vote
            {
                Name = "TestCreate",
                Description = "Ceci est un vote de test pour la création",
                Visibility = "public",
                Type = "plural",
                NbRounds = 1,
                WinnersByRound = new List<int>(),
                VictoryCondition = "invalid",
                ReplayOnDraw = false,
                Rounds = new List<Round>(),
                Options = new List<Option>()
            };


            Assert.Throws<DbUpdateException>(() =>
            {
                context.Votes.Add(vote);
                context.SaveChanges();
            });
        }
    }

    [Fact]
    public void ConstraintKey_Visibility_ThrowError()
    {
        using (var context = new DatabaseContext())
        {
            var vote = new Vote
            {
                Name = "InvalidVisibility",
                Description = "Test invalid visibility constraint",
                WinnersByRound = new List<int>(),
                Visibility = "invalid",
                Type = "plural",
                NbRounds = 1,
                VictoryCondition = "majority",
                ReplayOnDraw = false,
                Rounds = new List<Round>(),
                Options = new List<Option>()


            };

            Assert.Throws<DbUpdateException>(() =>
            {
                context.Votes.Add(vote);
                context.SaveChanges();
            });
        }
    }

    [Fact]
    public void ConstraintKey_Type_ThrowError()
    {
        using (var context = new DatabaseContext())
        {
            var vote = new Vote
            {
                Name = "InvalidType",
                Description = "Test invalid type constraint",
                WinnersByRound = new List<int>(),
                Visibility = "public",
                Type = "invalid",
                NbRounds = 1,
                VictoryCondition = "majority",
                ReplayOnDraw = false,
                Rounds = new List<Round>(),
                Options = new List<Option>()
            };

            Assert.Throws<DbUpdateException>(() =>
            {
                context.Votes.Add(vote);
                context.SaveChanges();
            });
        }
    }

    [Fact]
    public void ConstraintKey_Required_RoundOption_Round_ThrowError()
    {
        using (var context = new DatabaseContext())
        {
            var vote = new Vote
            {
                Name = "Test Vote",
                Visibility = "public",
                Type = "plural",
                Description = "Description du vote",
                ReplayOnDraw = false,
                NbRounds = 1,
                WinnersByRound = new List<int>(),
                VictoryCondition = "majority",
                Rounds = new List<Round>(),
                Options = new List<Option>()
            };

            context.Votes.Add(vote);
            context.SaveChanges();

            var round = new Round
            {
                Name = "Test Round",
                StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                EndTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds(),
                idVote = 999
            };

            Assert.Throws<DbUpdateException>(() =>
            {
                context.Rounds.Add(round);
                context.SaveChanges();
            });
        }
    }

    [Fact]
    public void ForeignKey_RoundOption_Option_ThrowError()
    {
        using (var context = new DatabaseContext())
        {
            var vote = new Vote
            {
                Name = "Test Vote",
                Visibility = "public",
                Type = "plural",
                Description = "Description du vote",
                ReplayOnDraw = false,
                NbRounds = 1,
                WinnersByRound = new List<int>(),
                VictoryCondition = "majority",
                Rounds = new List<Round>(),
                Options = new List<Option>()
            };

            context.Votes.Add(vote);
            context.SaveChanges();

            var option = new Option
            {
                Name = "Test Option",
                VoteId = 999
            };

            Assert.Throws<DbUpdateException>(() =>
            {
                context.Options.Add(option);
                context.SaveChanges();
            });
        }
    }

    [Fact]
    public void ForeignKey_RoundOption_Round_ThrowError()
    {
        using (var context = new DatabaseContext())
        {
            var vote = new Vote
            {
                Name = "Test Vote",
                Visibility = "public",
                Type = "plural",
                Description = "Description du vote",
                ReplayOnDraw = false,
                NbRounds = 1,
                WinnersByRound = new List<int>(),
                VictoryCondition = "majority",
                Rounds = new List<Round>(),
                Options = new List<Option>()
            };

            context.Votes.Add(vote);
            context.SaveChanges();

            var roundOption = new RoundOption
            {
                OptionId = 999,
                RoundId = 999
            };

            Assert.Throws<DbUpdateException>(() =>
            {
                context.RoundOptions.Add(roundOption);
                context.SaveChanges();
            });
        }
    }

    [Fact]
    public void ForeignKey_Round_Vote_ThrowError()
    {
        using (var context = new DatabaseContext())
        {
            var round = new Round
            {
                Name = "Test Round",
                StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                EndTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds(),
                idVote = 999
            };

            Assert.Throws<DbUpdateException>(() =>
            {
                context.Rounds.Add(round);
                context.SaveChanges();
            });
        }
    }


    [Fact]
    public void ForeignKey_Option_Vote_ThrowError()
    {
        using (var context = new DatabaseContext())
        {
            var option = new Option
            {
                Name = "Test Option",
                VoteId = 999
            };

            Assert.Throws<DbUpdateException>(() =>
            {
                context.Options.Add(option);
                context.SaveChanges();
            });
        }
    }
}
