namespace Domain
{
    public class VotingSystemBase : IVotingSystem
    {
        public EVotingSystems Type;
        private IVotinSystemStrategy _voteStrategy = new PluralVoteStrategy();
        private List<VoteOption> VoteOptions = new();
        private int NbRounds;
        public int currentRound = 0;
        public List<Round> Rounds = new();
        private int[] QualifiedPerRound;
        private EVictorySettings VictoryType;
        private bool RunAgainIfDraw;
        public string winnerName = "No winner";

        public VotingSystemBase(EVotingSystems type, List<VoteOption> voteOptions, int nbRounds, int[] qualifiedPerRound, EVictorySettings victorySettings, bool runAgainIfDraw)
        {
            Type = type;
            SetVoteSystemStrategy(type);
            NbRounds = nbRounds;
            QualifiedPerRound = qualifiedPerRound;
            VictoryType = victorySettings;
            RunAgainIfDraw = runAgainIfDraw;

            foreach (VoteOption option in voteOptions)
            {
                AddCandidate(option.Name, option.Description);
            }

        }

        public virtual void AddCandidate(string candidateName, string candidateDescription)
        {
            VoteOptions.Add(new(candidateName, candidateDescription));
        }

        public virtual void AddVote(string canditateName, int scoreToAdd)
        {
            var entry = Rounds[currentRound - 1].VoteOptions.FirstOrDefault(cs => cs.Name == canditateName);

            if (entry != null)
            {
                entry.Score += scoreToAdd;
            }
            else
            {
                Console.WriteLine($"Candidat '{canditateName}' non trouvé.");
            }
        }

        public EResult GetResult(int roundNumber)
        {
            if (Rounds[roundNumber - 1].VoteOptions.Count == 0)
                return EResult.Inconclusive;

            int maxScore = Rounds[roundNumber - 1].VoteOptions.Max(vos => vos.Score);
            int countMax = Rounds[roundNumber - 1].VoteOptions.Count(vos => vos.Score == maxScore);

            if (maxScore == 0)
                return EResult.Inconclusive;

            if (countMax > 1)
                return EResult.Draw;

            if (countMax == 1)
            {
                winnerName = Rounds[roundNumber - 1].VoteOptions.First(vos => vos.Score == maxScore).Name;
                return EResult.Winner;
            }

            return EResult.Inconclusive;
        }

        public void NextRound()
        {
            if (currentRound >= NbRounds)
                return;

            if (currentRound == 0)
            {
                Rounds.Add(new Round(VoteOptions));
            }
            else
            {
                Rounds.Add(new Round(DetermineQualified(QualifiedPerRound[currentRound - 1])));
            }

            currentRound++;
        }



        private List<VoteOption> DetermineQualified(int nbQualified)
        {
            return GetStanding()
                .Take(nbQualified)
                .Select(vo => new VoteOption(vo.Name, vo.Description))
                .ToList();
        }


        private List<VoteOption> GetStanding()
        {
            return Rounds[currentRound - 1].VoteOptions
                .OrderByDescending(vo => vo.Score)
                .ToList();
        }


        private void SetVoteSystemStrategy(EVotingSystems type)
        {
            switch (type)
            {
                case EVotingSystems.Plural:
                    _voteStrategy = new PluralVoteStrategy();
                    break;
            }
        }

        private void SetVictoryStrategy(EVotingSystems type)
        {
            switch (type)
            {
                case EVotingSystems.Plural:
                    _voteStrategy = new PluralVoteStrategy();
                    break;
            }
        }
    }
    

}