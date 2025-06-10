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

        public VotingSystemBase(EVotingSystems type, List<VoteOption> voteOptions, int nbRounds, int[] qualifiedPerRound, EVictorySettings victorySettings, bool runAgainIfDraw, List<Round> rounds)
        {
            Type = type;
            SetVoteSystemStrategy(type);
            VoteOptions = voteOptions ?? throw new ArgumentNullException(nameof(voteOptions), "Vote options cannot be null.");
            NbRounds = nbRounds;
            QualifiedPerRound = qualifiedPerRound;
            VictoryType = victorySettings;
            SetVictoryStrategy(victorySettings);
            RunAgainIfDraw = runAgainIfDraw;
            Rounds = rounds ?? throw new ArgumentNullException(nameof(rounds), "Rounds cannot be null.");

            if (Rounds.Count == 0)
                CreateInitialRounds();
        }

        private void CreateInitialRounds()
        {
            NextRound();
        }

        public virtual void AddDecision(Decision decision, int? roundNumber = null)
        {
            if (decision == null)
                throw new ArgumentNullException(nameof(decision), "Decision cannot be null.");
            int roundIndex = currentRound - 1;
            if (roundNumber != null)
            {
                roundIndex = roundNumber.Value - 1;
            }
            if (roundIndex < 0 || roundIndex >= Rounds.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(roundNumber), "Round number is out of range.");
            }

            Rounds[roundIndex].AddVote(decision.Id, decision.Score);
        }

        public EResult GetRoundResult(int roundNumber)
        {
            return Rounds[roundNumber - 1].GetResult();
        }

        public List<int> GetVoteWinner()
        {
            return _victoryStrategy.GetWinner(Rounds[currentRound - 1].VoteOptions);
        }

        public bool IsVoteOver()
        {
            bool isLastRound = currentRound >= NbRounds;
            bool isMajorityVictory = VictoryType == EVictorySettings.Absolute_Majority || VictoryType == EVictorySettings.TwoThirds_Majority;

            return (isLastRound && !RunAgainIfDraw) ||
                (currentRound > 0 && isMajorityVictory && GetRoundResult(currentRound) == EResult.Winner) ||
                (currentRound == NbRounds && GetRoundResult(currentRound) == EResult.Winner);
        }

        public bool NextRound()
        {
            if (IsVoteOver())
            {
                return false;
            }

            if (currentRound == 0)
            {
                Rounds.Add(new Round(VoteOptions, _victoryStrategy));
                currentRound++;
                return true;
            }

            int roundIndex = currentRound - 1;

            if (currentRound == NbRounds && RunAgainIfDraw && GetRoundResult(currentRound) == EResult.Draw)
            {
                var previousRound = Rounds[roundIndex];
                int maxScore = previousRound.VoteOptions.Max(v => v.Score);

                var drawCandidates = previousRound.VoteOptions
                    .Where(v => v.Score == maxScore)
                    .ToList();

                Rounds.Add(new Round(drawCandidates, _victoryStrategy));
                RunAgainIfDraw = false;
            }
            else
            {
                var qualified = DetermineQualified(QualifiedPerRound[roundIndex]);
                Rounds.Add(new Round(qualified, _victoryStrategy));
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
                .Select(vo => new VoteOption(vo.Id, vo.Name))
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
