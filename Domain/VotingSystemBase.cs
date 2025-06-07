namespace Domain
{
    public class VotingSystemBase : IVotingSystem
    {
        public EVotingSystems Type;
        private IVotinSystemStrategy _voteStrategy = new PluralVoteStrategy();
        public IVictoryStrategy _victoryStrategy = new RelativeMajorityStrategy();
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
            SetVictoryStrategy(victorySettings);
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
            return this._victoryStrategy.CheckResult(Rounds[roundNumber - 1]);
        }

        public string GetWinner()
        {
            return this._victoryStrategy.GetWinner(Rounds[currentRound - 1]);
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
                case EVotingSystems.Weighted:
                    _voteStrategy = new WeightedVoteStrategy();
                    break;
                case EVotingSystems.Ranked:
                    _voteStrategy = new RankedVoteStrategy();
                    break;
                case EVotingSystems.ELO:
                    _voteStrategy = new ELOVoteStrategy();
                    break;
            }
        }

        private void SetVictoryStrategy(EVictorySettings type)
        {
            switch (type)
            {
                case EVictorySettings.Relative_Majority:
                    _victoryStrategy = new RelativeMajorityStrategy();
                    break;
                case EVictorySettings.Absolute_Majority:
                    _victoryStrategy = new AbsoluteMajorityStrategy();
                    break;
                case EVictorySettings.TwoThirds_Majority:
                    _victoryStrategy = new TwoThirdsMajorityStrategy();
                    break;
                case EVictorySettings.None:
                    _victoryStrategy = new NoVictoryStrategy();
                    break;
                case EVictorySettings.LastManStanding:
                    _victoryStrategy = new BRVictoryStrategy();
                    break;
            }
        }
    }
    

}