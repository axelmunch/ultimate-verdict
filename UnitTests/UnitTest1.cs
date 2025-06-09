namespace UnitTests;

using System.Diagnostics;
using Domain;

public class PluralVoteTests
{

    public static IEnumerable<object[]> StructuredCandidateTestData =>
    new List<object[]>
    {
        //=====================================================================Majorité relative=====================================================================
        // Test 1 – majorité relative simple, Alice gagne
        new object[]
        {
            EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice", new List<int> { 93 }),
                new(9255, "Bob",   new List<int> { 12 })
            },
            EResult.Winner,
            "Alice"
        },

        // Test 2 – égalité
        new object[]
        {
            EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice", new List<int> { 50 }),
                new(9255, "Bob",   new List<int> { 50 })
            },
            EResult.Draw,
            "Alice, Bob"
        },

        // Test 3 – Inconclusif
        new object[]
        {
            EVotingSystems.Plural, 2, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice", new List<int> { 0 }),
                new(9255, "Bob",   new List<int> { 0 })
            },
            EResult.Inconclusive,
            "Alice, Bob"
        },

        // Test 4 – deux tours, Alice bat David
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 154 }),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98, 140 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            "Alice"
        },

        // Test 5 – deux tours, egalité
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 140 }),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98, 140 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Draw,
            "David, Alice"
        },

        // Test 5 – deux tours, egalité puis victoire
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 140, 145 }),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98, 140, 142 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            "Alice"
        },


        //Test : 2 tour, 3 selectionné car égalité
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 188, 251 }),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 43, 55 }),
                new(2563, "Emma",    new List<int> { 43, 69 }),
            },
            EResult.Winner,
            "Alice"
        },

        //Test : 2 tour, 3 selectionné car égalité, puis égalité
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 188, 55 }),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 43, 55 }),
                new(2563, "Emma",    new List<int> { 43, 55 }),
            },
            EResult.Draw,
            "Alice, David, Emma"
        },


        //Test : 1 tour, égalité parfaite, puis à nouveau égalité
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 52, 52 }),
                new(9255, "Bob",     new List<int> { 52, 52 }),
                new(6123, "Charlie", new List<int> { 52, 52 }),
                new(4534, "David",   new List<int> { 52, 52 }),
                new(2563, "Emma",    new List<int> { 52, 52 }),
            },
            EResult.Draw,
            "Alice, Bob, Charlie, David, Emma"
        },          

        //=====================================================================Majorité absolue=====================================================================
        //Test 6 : 1 tour, inconcluant
        new object[]
        {
            EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93}),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Inconclusive,
            "Alice, Bob, Charlie, David, Emma"
        },

        //Test 7 : 1 tour, inconcluant
        new object[]
        {
            EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43 }),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 21 }),
                new(2563, "Emma",    new List<int> { 188 }),
            },
            EResult.Winner,
            "Emma"
        },

        //Test 7 : 1 tour, Egalité
        new object[]
        {
            EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43 }),
                new(9255, "Bob",     new List<int> { 123 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 21 }),
                new(2563, "Emma",    new List<int> { 123 }),
            },
            EResult.Draw,
            "Bob, Emma"
        },

        //Test : 2 tour, victoire au deuxieme
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 114 }),
                new(9255, "Bob",     new List<int> { 61 }),
                new(6123, "Charlie", new List<int> { 37 }),
                new(4534, "David",   new List<int> { 108, 106 }),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Winner,
            "Alice"
        },

        //Test 2 tour, victoire 1er tour
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43}),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 188 }),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Winner,
            "David"
        },

        //Test 2 tour, Egalité sans relance
         new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 110 }),
                new(9255, "Bob",     new List<int> { 61 }),
                new(6123, "Charlie", new List<int> { 37 }),
                new(4534, "David",   new List<int> { 108, 110 }),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Draw,
            "David, Alice"
        },

        //Test 2 tour, Egalité, victoire  relance
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Absolute_Majority, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 110, 109 }),
                new(9255, "Bob",     new List<int> { 61 }),
                new(6123, "Charlie", new List<int> { 37 }),
                new(4534, "David",   new List<int> { 108, 110, 111 }),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Winner,
            "David"
        },

        //Test 2 tour, egalité, relance égalité
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.Absolute_Majority, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 110, 120 }),
                new(9255, "Bob",     new List<int> { 61 }),
                new(6123, "Charlie", new List<int> { 37 }),
                new(4534, "David",   new List<int> { 108, 110, 120 }),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Draw,
            "David, Alice"
        },


        //=====================================================================Majorité 2/3=====================================================================
        //Test 8 : 1 tour, Inconcluant
        new object[]
        {
            EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.TwoThirds_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93 }),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Inconclusive,
            "Alice, Bob, Charlie, David, Emma"
        },

        //Test 9 : 1 tour, Vainqueur
        new object[]
        {
            EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.TwoThirds_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 33 }),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 218 }),
                new(4534, "David",   new List<int> { 7 }),
                new(2563, "Emma",    new List<int> { 11 }),
            },
            EResult.Winner,
            "Charlie"
        },

        //Test 9 : 1 tour, Vainqueur
        new object[]
        {
            EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.TwoThirds_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43 }),
                new(9255, "Bob",     new List<int> { 123 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 21 }),
                new(2563, "Emma",    new List<int> { 123 }),
            },
            EResult.Draw,
            "Bob, Emma"
        },

        //Test 9 : 2 tour, pas de maj
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 3, 1 }, EVictorySettings.TwoThirds_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43 , 10}),
                new(9255, "Bob",     new List<int> { 123, 119}),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 21 }),
                new(2563, "Emma",    new List<int> { 123 , 101 }),
            },
            EResult.Inconclusive,
            "Bob, Emma, Alice"
        },

        //Test 9 : 2 tour, pas de maj x2 (égalité)
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.TwoThirds_Majority, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43 }),
                new(9255, "Bob",     new List<int> { 123, 110, 110}),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 21 }),
                new(2563, "Emma",    new List<int> { 123, 110, 100 }),
            },
            EResult.Inconclusive,
            "Bob, Emma"
        },

        //Test 9 : 2 tour, victoire 2e tour
        new object[]
        {
            EVotingSystems.Plural, 5, 2, new int[] { 2, 1 }, EVictorySettings.TwoThirds_Majority, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 88, 251 }),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 43 , 69}),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Winner,
            "Alice"
        },




        //===================================================================== Aucune condition de victoire =====================================================================
                //Test 8 : 1 tour, Inconcluant
        new object[]
        {
            EVotingSystems.Plural, 5, 1, new int[] { 1 }, EVictorySettings.None, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 }),
                new(9255, "Bob",     new List<int> { 62 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.None,
            "Alice, Bob, Charlie, David, Emma"
        },


        //===================================================================== Last Man Standing =====================================================================
        //Test  : Winner
        new object[]
        {
            EVotingSystems.Plural, 5, 4, new int[] { 4, 3, 2, 1}, EVictorySettings.LastManStanding, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 , 84, 115, 131}),
                new(9255, "Bob",     new List<int> { 62 , 73, 100}),
                new(6123, "Charlie", new List<int> { 17 , 26}),
                new(4534, "David",   new List<int> { 98 , 100, 129, 130}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            "Alice"
        },

        //Test  : Draw
        new object[]
        {
            EVotingSystems.Plural, 5, 4, new int[] { 4, 3, 2, 1}, EVictorySettings.LastManStanding, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 , 84, 115, 130}),
                new(9255, "Bob",     new List<int> { 62 , 73, 100}),
                new(6123, "Charlie", new List<int> { 17 , 26}),
                new(4534, "David",   new List<int> { 98 , 100, 129, 130}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Draw,
            "David, Alice"
        },

        //Test  : Egalité dernier duel puis victoire
        new object[]
        {
            EVotingSystems.Plural, 5, 4, new int[] { 4, 3, 2, 1}, EVictorySettings.LastManStanding, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 , 84, 115, 130, 125}),
                new(9255, "Bob",     new List<int> { 62 , 73, 100}),
                new(6123, "Charlie", new List<int> { 17 , 26}),
                new(4534, "David",   new List<int> { 98 , 100, 129, 130, 135}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            "David"
        },

        //Test  : Egalité dernier duel, égalité
        new object[]
        {
            EVotingSystems.Plural, 5, 4, new int[] { 4, 3, 2, 1}, EVictorySettings.LastManStanding, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 , 84, 115, 130, 135}),
                new(9255, "Bob",     new List<int> { 62 , 73, 100}),
                new(6123, "Charlie", new List<int> { 17 , 26}),
                new(4534, "David",   new List<int> { 98 , 100, 129, 130, 135}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Draw,
            "David, Alice"
        },


        //Test  : Egalité au cours d'une manche
        new object[]
        {
            EVotingSystems.Plural, 5, 4, new int[] { 4, 3, 2, 1}, EVictorySettings.LastManStanding, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 , 84, 115, 135}),
                new(9255, "Bob",     new List<int> { 62 , 73, 100}),
                new(6123, "Charlie", new List<int> { 17 , 73, 74}),
                new(4534, "David",   new List<int> { 98 , 100, 129, 112}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            "Alice"
        },
    };


    [Theory]
    [MemberData(nameof(StructuredCandidateTestData))]
    public void GlobalResultTest_WithStructuredCandidates(EVotingSystems votingType, int nbVoteOptions, int nbRounds, int[] qualifiedPerRound, EVictorySettings victorySettings, bool runAgainIfDraw, List<TestCandidate> candidates, EResult expectedResult, string expectedWinnerName)
    {
        var voteOptions = candidates
            .Take(nbVoteOptions)
            .Select(c => new VoteOption(c.Name, c.Description, c.Id))
            .ToList();

        var vote = new VotingSystemBase(votingType, voteOptions, nbRounds, qualifiedPerRound, victorySettings, runAgainIfDraw);

        while (vote.NextRound())
        {
            foreach (var vo in vote.Rounds[vote.currentRound - 1].VoteOptions)
            {
                var testData = candidates.First(c => c.Id == vo.Id);
                vote.AddVote(vo.Id, testData.ScoresPerRound[vote.currentRound - 1]);
            }
        }

        Assert.Equal(expectedResult, vote.GetRoundResult(vote.currentRound));
        Assert.Equal(expectedWinnerName, vote.GetVoteWinner());
    }
}