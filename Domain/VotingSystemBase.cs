namespace Domain
{
    public class VotingSystemBase : IVotingSystem
    {
        public EVotingSystems Type;
        public IVictoryStrategy _victoryStrategy = new RelativeMajorityStrategy();
        private List<Option> Options = new();
        private int NbRounds;
        public int currentRound = 0;
        public List<Round> Rounds = new();
        private int[] QualifiedPerRound;
        private EVictorySettings VictoryType;
        private bool RunAgainIfDraw;

        public VotingSystemBase(EVotingSystems type, List<Option> options, int nbRounds, int[] qualifiedPerRound, EVictorySettings victorySettings, bool runAgainIfDraw, List<Round> rounds)
        {
            if (options.Count < 2)
                throw new ArgumentException("Au moins deux candidats sont requis.");

            if (nbRounds <= 0)
                throw new ArgumentException("Le nombre de tours doit être supérieur à 0.");

            if (qualifiedPerRound == null || qualifiedPerRound.Length != nbRounds)
                throw new ArgumentException("qualifiedPerRound doit contenir une valeur par tour.");

            if (qualifiedPerRound.Any(q => q <= 0))
                throw new ArgumentException("Le nombre de candidats qualifiés par tour doit être supérieur à 0.");

            Type = type;
            Options = options ?? throw new ArgumentNullException(nameof(options), "Vote options cannot be null.");
            NbRounds = nbRounds;
            QualifiedPerRound = qualifiedPerRound;
            VictoryType = victorySettings;
            SetVictoryStrategy(victorySettings);
            RunAgainIfDraw = runAgainIfDraw;
            Rounds = rounds ?? throw new ArgumentNullException(nameof(rounds), "Rounds cannot be null.");
            currentRound = rounds.Count;

            if (Rounds.Count == 0)
                CreateInitialRounds();
        }

        private void CreateInitialRounds()
        {
            NextRound();
        }

        public virtual void AddDecision(List<Decision> decisions, int? roundNumber = null)
        {
            if (decisions == null)
                throw new ArgumentNullException(nameof(decisions), "Decision cannot be null.");

            int roundIndex = currentRound - 1;
            if (roundNumber != null)
            {
                roundIndex = roundNumber.Value - 1;
            }
            if (roundIndex < 0 || roundIndex >= Rounds.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(roundNumber), "Round number is out of range.");
            }

            new DecisionControl().Control(decisions, Type, Rounds[roundIndex].Options, false);
            foreach (var decision in decisions)
            {
                Rounds[roundIndex].AddVote(decision.Id, decision.Score);
            }
        }

        public EResult GetRoundResult(int roundNumber)
        {
            return Rounds[roundNumber - 1].GetResult();
        }

        public List<int> GetVoteWinner()
        {
            return _victoryStrategy.GetWinner(Rounds[currentRound - 1].Options);
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
                Rounds.Add(new Round(Options, _victoryStrategy));
                currentRound++;
                return true;
            }

            int roundIndex = currentRound - 1;

            if (currentRound == NbRounds && RunAgainIfDraw && GetRoundResult(currentRound) == EResult.Draw)
            {
                var previousRound = Rounds[roundIndex];
                int maxScore = previousRound.Options.Max(v => v.Score);

                var drawCandidates = previousRound.Options
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

        private List<Option> DetermineQualified(int nbQualified)
        {
            var standing = GetStanding();

            if (nbQualified > standing.Count)
                throw new InvalidOperationException($"Impossible de qualifier {nbQualified} candidats alors qu’il n’en reste que {standing.Count}.");

            int minQualifiedScore = standing.Take(nbQualified).Last().Score;

            return standing
                .Where(vo => vo.Score >= minQualifiedScore)
                .Select(vo => new Option(vo.Id, vo.Name))
                .ToList();
        }

        private List<Option> GetStanding()
        {
            return Rounds[currentRound - 1].Options
                .OrderByDescending(vo => vo.Score)
                .ToList();
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
