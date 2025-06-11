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
            EVotingSystems.Plural, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice", new List<int> { 93 }),
                new(9255, "Bob",   new List<int> { 12 })
            },
            EResult.Winner,
            new List<int>{8765 }
        },

        // Test 2 – égalité
        new object[]
        {
            EVotingSystems.Plural, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice", new List<int> { 50 }),
                new(9255, "Bob",   new List<int> { 50 })
            },
            EResult.Draw,
            new List<int>{8765, 9255 }
        },

        // Test 3 – Inconclusif
        new object[]
        {
            EVotingSystems.Plural, 1, new int[] { 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice", new List<int> { 0 }),
                new(9255, "Bob",   new List<int> { 0 })
            },
            EResult.Inconclusive,
            new List<int>{8765, 9255 }
        },

        // Test 4 – deux tours, Alice bat David
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 154 }),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98, 140 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            new List<int>{8765 }
        },

        // Test 5 – deux tours, egalité
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 140 }),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98, 140 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Draw,
            new List<int>{4534, 8765 }
        },

        // Test 5 – deux tours, egalité puis victoire
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 140, 145 }),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98, 140, 142 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            new List<int>{8765 }
        },


        //Test : 2 tour, 3 selectionné car égalité
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 188, 251 }),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 43, 55 }),
                new(2563, "Emma",    new List<int> { 43, 69 }),
            },
            EResult.Winner,
           new List<int>{ 8765 }
        },

        //Test : 2 tour, 3 selectionné car égalité, puis égalité
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 188, 55 }),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 43, 55 }),
                new(2563, "Emma",    new List<int> { 43, 55 }),
            },
            EResult.Draw,
            new List<int>{8765, 4534, 2563 }
        },


        //Test : 1 tour, égalité parfaite, puis à nouveau égalité
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.Relative_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 52, 52 }),
                new(9255, "Bob",     new List<int> { 52, 52 }),
                new(6123, "Charlie", new List<int> { 52, 52 }),
                new(4534, "David",   new List<int> { 52, 52 }),
                new(2563, "Emma",    new List<int> { 52, 52 }),
            },
            EResult.Draw,
            new List<int>{8765, 9255, 6123, 4534, 2563 }
        },


        //=====================================================================Majorité absolue=====================================================================
        //Test 6 : 1 tour, inconcluant
        new object[]
        {
            EVotingSystems.Plural, 1, new int[] { 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93}),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Inconclusive,
            new List<int>{8765, 9255, 6123, 4534, 2563 }
        },

        //Test 7 : 1 tour, inconcluant
        new object[]
        {
            EVotingSystems.Plural, 1, new int[] { 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43 }),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 21 }),
                new(2563, "Emma",    new List<int> { 188 }),
            },
            EResult.Winner,
            new List<int>{2563 }
        },

        //Test 7 : 1 tour, Egalité
        new object[]
        {
            EVotingSystems.Plural, 1, new int[] { 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43 }),
                new(9255, "Bob",     new List<int> { 123 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 21 }),
                new(2563, "Emma",    new List<int> { 123 }),
            },
            EResult.Draw,
            new List<int>{9255, 2563 }
        },

        //Test : 2 tour, victoire au deuxieme
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 114 }),
                new(9255, "Bob",     new List<int> { 61 }),
                new(6123, "Charlie", new List<int> { 37 }),
                new(4534, "David",   new List<int> { 108, 106 }),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Winner,
            new List<int>{8765 }
        },

        //Test 2 tour, victoire 1er tour
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43}),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 188 }),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Winner,
            new List<int>{4534 }
        },

        //Test 2 tour, Egalité sans relance
         new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.Absolute_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 110 }),
                new(9255, "Bob",     new List<int> { 61 }),
                new(6123, "Charlie", new List<int> { 37 }),
                new(4534, "David",   new List<int> { 108, 110 }),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Draw,
            new List<int>{4534, 8765 }
        },

        //Test 2 tour, Egalité, victoire  relance
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.Absolute_Majority, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 110, 109 }),
                new(9255, "Bob",     new List<int> { 61 }),
                new(6123, "Charlie", new List<int> { 37 }),
                new(4534, "David",   new List<int> { 108, 110, 111 }),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Winner,
            new List<int>{4534 }
        },

        //Test 2 tour, egalité, relance égalité
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.Absolute_Majority, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93, 110, 120 }),
                new(9255, "Bob",     new List<int> { 61 }),
                new(6123, "Charlie", new List<int> { 37 }),
                new(4534, "David",   new List<int> { 108, 110, 120 }),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Draw,
            new List<int>{4534, 8765 }
        },


        //=====================================================================Majorité 2/3=====================================================================
        //Test 8 : 1 tour, Inconcluant
        new object[]
        {
            EVotingSystems.Plural, 1, new int[] { 1 }, EVictorySettings.TwoThirds_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 93 }),
                new(9255, "Bob",     new List<int> { 42 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Inconclusive,
            new List<int>{8765, 9255, 6123, 4534, 2563 }
        },

        //Test 9 : 1 tour, Vainqueur
        new object[]
        {
            EVotingSystems.Plural, 1, new int[] { 1 }, EVictorySettings.TwoThirds_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 33 }),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 218 }),
                new(4534, "David",   new List<int> { 7 }),
                new(2563, "Emma",    new List<int> { 11 }),
            },
            EResult.Winner,
            new List<int>{6123 }
        },

        //Test 9 : 1 tour, Vainqueur
        new object[]
        {
            EVotingSystems.Plural, 1, new int[] { 1 }, EVictorySettings.TwoThirds_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43 }),
                new(9255, "Bob",     new List<int> { 123 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 21 }),
                new(2563, "Emma",    new List<int> { 123 }),
            },
            EResult.Draw,
            new List<int>{9255, 2563 }
        },

        //Test 9 : 2 tour, pas de maj
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 3, 1 }, EVictorySettings.TwoThirds_Majority, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43 , 10}),
                new(9255, "Bob",     new List<int> { 123, 119}),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 21 }),
                new(2563, "Emma",    new List<int> { 123 , 101 }),
            },
            EResult.Inconclusive,
            new List<int>{9255, 2563, 8765 }
        },

        //Test 9 : 2 tour, pas de maj x2 (égalité)
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.TwoThirds_Majority, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 43 }),
                new(9255, "Bob",     new List<int> { 123, 110, 110}),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 21 }),
                new(2563, "Emma",    new List<int> { 123, 110, 100 }),
            },
            EResult.Inconclusive,
            new List<int>{9255, 2563 }
        },

        //Test 9 : 2 tour, victoire 2e tour
        new object[]
        {
            EVotingSystems.Plural, 2, new int[] { 2, 1 }, EVictorySettings.TwoThirds_Majority, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 88, 251 }),
                new(9255, "Bob",     new List<int> { 11 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 43 , 69}),
                new(2563, "Emma",    new List<int> { 21 }),
            },
            EResult.Winner,
            new List<int>{8765}
        },


        //===================================================================== Aucune condition de victoire =====================================================================
        //Test 8 : 1 tour, Inconcluant
        new object[]
        {
            EVotingSystems.Plural, 1, new int[] { 1 }, EVictorySettings.None, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 }),
                new(9255, "Bob",     new List<int> { 62 }),
                new(6123, "Charlie", new List<int> { 17 }),
                new(4534, "David",   new List<int> { 98 }),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.None,
            new List<int>{8765, 9255, 6123, 4534, 2563 }
        },


        //===================================================================== Last Man Standing =====================================================================
        //Test  : Winner
        new object[]
        {
            EVotingSystems.Plural, 4, new int[] { 4, 3, 2, 1}, EVictorySettings.LastManStanding, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 , 84, 115, 131}),
                new(9255, "Bob",     new List<int> { 62 , 73, 100}),
                new(6123, "Charlie", new List<int> { 17 , 26}),
                new(4534, "David",   new List<int> { 98 , 100, 129, 130}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            new List<int>{8765}
        },

        //Test  : Draw
        new object[]
        {
            EVotingSystems.Plural, 4, new int[] { 4, 3, 2, 1}, EVictorySettings.LastManStanding, false,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 , 84, 115, 130}),
                new(9255, "Bob",     new List<int> { 62 , 73, 100}),
                new(6123, "Charlie", new List<int> { 17 , 26}),
                new(4534, "David",   new List<int> { 98 , 100, 129, 130}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Draw,
            new List<int>{4534, 8765 }
        },

        //Test  : Egalité dernier duel puis victoire
        new object[]
        {
            EVotingSystems.Plural, 4, new int[] { 4, 3, 2, 1}, EVictorySettings.LastManStanding, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 , 84, 115, 130, 125}),
                new(9255, "Bob",     new List<int> { 62 , 73, 100}),
                new(6123, "Charlie", new List<int> { 17 , 26}),
                new(4534, "David",   new List<int> { 98 , 100, 129, 130, 135}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            new List<int>{4534 }
        },

        //Test  : Egalité dernier duel, égalité
        new object[]
        {
            EVotingSystems.Plural, 4, new int[] { 4, 3, 2, 1}, EVictorySettings.LastManStanding, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 , 84, 115, 130, 135}),
                new(9255, "Bob",     new List<int> { 62 , 73, 100}),
                new(6123, "Charlie", new List<int> { 17 , 26}),
                new(4534, "David",   new List<int> { 98 , 100, 129, 130, 135}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Draw,
            new List<int>{4534, 8765}
        },


        //Test  : Egalité au cours d'une manche
        new object[]
        {
            EVotingSystems.Plural, 4, new int[] { 4, 3, 2, 1}, EVictorySettings.LastManStanding, true,
            new List<TestCandidate>
            {
                new(8765, "Alice",   new List<int> { 62 , 84, 115, 135}),
                new(9255, "Bob",     new List<int> { 62 , 73, 100}),
                new(6123, "Charlie", new List<int> { 17 , 73, 74}),
                new(4534, "David",   new List<int> { 98 , 100, 129, 112}),
                new(2563, "Emma",    new List<int> { 10 }),
            },
            EResult.Winner,
            new List<int>{8765}
        },
    };


    [Theory]
    [MemberData(nameof(StructuredCandidateTestData))]
    public void GlobalResultTest_WithStructuredCandidates(EVotingSystems votingType, int nbRounds, int[] qualifiedPerRound, EVictorySettings victorySettings, bool runAgainIfDraw, List<TestCandidate> candidates, EResult expectedResult, List<int> expectedWinnerID)
    {
        var options = candidates
            .Take(candidates.Count)
            .Select(c => new Option(c.Id, c.Name))
            .ToList();

        var vote = new VotingSystemBase(votingType, options, nbRounds, qualifiedPerRound, victorySettings, runAgainIfDraw, new List<Round>());


        do
        {
            var roundDecisions = new List<Decision>();

            int roundIndex = vote.currentRound - 1;

            // Recreating decisions
            foreach (var roundOption in vote.Rounds[roundIndex].Options)
            {
                roundDecisions.Add(new Decision(roundOption.Id, candidates.First(c => c.Id == roundOption.Id).ScoresPerRound[roundIndex]));
            }

            foreach (var decision in roundDecisions)
            {
                vote.AddDecision(decision, vote.currentRound);
            }
        }
        while (vote.NextRound());

        Assert.Equal(expectedResult, vote.GetRoundResult(vote.currentRound));
        Assert.Equal(expectedWinnerID, vote.GetVoteWinner());
    }
}
