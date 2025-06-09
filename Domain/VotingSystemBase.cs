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
        public bool isOver = false;

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
                AddCandidate(option.Name, option.Id);
            }

        }

        public virtual void AddCandidate(string candidateName, int id)
        {
            VoteOptions.Add(new(candidateName, id));
        }

        public virtual void AddVote(int id, int scoreToAdd)
        {
            Rounds[currentRound - 1].AddVote(id, scoreToAdd);
        }

        public EResult GetRoundResult(int roundNumber)
        {
            return Rounds[roundNumber - 1].GetResult();
        }

        public List<int> GetVoteWinner()
        {
            return _victoryStrategy.GetWinner(Rounds[currentRound - 1]);
        }


        public bool NextRound()
        {
            bool isLastRound = currentRound >= NbRounds;
            bool isMajorityVictory = VictoryType == EVictorySettings.Absolute_Majority || VictoryType == EVictorySettings.TwoThirds_Majority;

            bool endCondition =
                (isLastRound && !RunAgainIfDraw) ||
                (currentRound > 0 && isMajorityVictory && GetRoundResult(currentRound) == EResult.Winner) ||
                (currentRound == NbRounds && GetRoundResult(currentRound) == EResult.Winner);

            if (endCondition)
            {
                isOver = true;
                return false;
            }

            if (currentRound == 0)
                Rounds.Add(new Round(VoteOptions, _victoryStrategy, _voteStrategy));

            else if (currentRound == NbRounds && RunAgainIfDraw && GetRoundResult(currentRound) == EResult.Draw)
            {
                var previousRound = Rounds[currentRound - 1];
                int maxScore = previousRound.VoteOptions.Max(v => v.Score);

                var drawCandidates = previousRound.VoteOptions
                    .Where(v => v.Score == maxScore)
                    .Select(v => new VoteOption(v.Name, v.Id))
                    .ToList();

                Rounds.Add(new Round(drawCandidates, _victoryStrategy, _voteStrategy));
                RunAgainIfDraw = false;
            }
            else
            {
                var qualified = DetermineQualified(QualifiedPerRound[currentRound - 1]);
                Rounds.Add(new Round(qualified, _victoryStrategy, _voteStrategy));
            }

            currentRound++;
            return true;
        }


        //TODO: A passer dnas le round une fois class result finie
        private List<VoteOption> DetermineQualified(int nbQualified)
        {
            var standing = GetStanding();
            int minQualifiedScore = standing.Take(nbQualified).Last().Score;

            return standing
                .Where(vo => vo.Score >= minQualifiedScore)
                .Select(vo => new VoteOption(vo.Name, vo.Id))
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
