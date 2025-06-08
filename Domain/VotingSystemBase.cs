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
                AddCandidate(option.Name, option.Description, option.Id);
            }

        }

        public virtual void AddCandidate(string candidateName, string candidateDescription, int id)
        {
            VoteOptions.Add(new(candidateName, candidateDescription, id));
        }

        public virtual void AddVote(int id, int scoreToAdd)
        {
            Rounds[currentRound - 1].AddVote(id, scoreToAdd);
        }

        public EResult GetRoundResult(int roundNumber)
        {
            return Rounds[roundNumber - 1].GetResult();
        }

        public string GetVoteWinner()
        {
            return _victoryStrategy.GetWinner(Rounds[currentRound - 1]);
        }


        public void NextRound()
        {
            if (currentRound >= NbRounds)
                return;

            if (currentRound == 0)
            {
                Rounds.Add(new Round(VoteOptions, _victoryStrategy, _voteStrategy));
            }
            else
            {
                Rounds.Add(new Round(DetermineQualified(QualifiedPerRound[currentRound - 1]), _victoryStrategy, _voteStrategy));
            }

            currentRound++;
        }


        //TODO: A passer dnas le round une fois class result finie
        private List<VoteOption> DetermineQualified(int nbQualified)
        {
            return GetStanding()
                .Take(nbQualified)
                .Select(vo => new VoteOption(vo.Name, vo.Description, vo.Id))
                .ToList();
        }

        //TODO: A passer dnas le round une fois class result finie
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