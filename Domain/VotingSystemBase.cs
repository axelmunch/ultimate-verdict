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


        public bool NextRound()
        {
            if (currentRound >= NbRounds && !RunAgainIfDraw)
            {
                Console.WriteLine("[DEBUG] NextRound - Cas 1 : fin sans relance");

                isOver = true;
                return false;
            }

            if (currentRound == 0)
            {
                Console.WriteLine("[DEBUG] NextRound - Cas 2 : premier tour");
                Rounds.Add(new Round(VoteOptions, _victoryStrategy, _voteStrategy));
            }

            else if (currentRound == NbRounds &&
                    GetRoundResult(currentRound) == EResult.Draw &&
                    RunAgainIfDraw)
            {
                Console.WriteLine("[DEBUG] NextRound - Cas 3 : égalité + relance");

                var previousRound = Rounds[currentRound - 1];
                int maxScore = previousRound.VoteOptions.Max(v => v.Score);

                var drawCandidates = previousRound.VoteOptions
                    .Where(v => v.Score == maxScore)
                    .Select(v => new VoteOption(v.Name, v.Description, v.Id))
                    .ToList();

                Rounds.Add(new Round(drawCandidates, _victoryStrategy, _voteStrategy));
                RunAgainIfDraw = false;
            }

            else
            {
                Console.WriteLine("[DEBUG] NextRound - Cas 4 : tour normal avec qualification");

                var qualified = DetermineQualified(QualifiedPerRound[currentRound - 1]);
                Rounds.Add(new Round(qualified, _victoryStrategy, _voteStrategy));
            }

            currentRound++;
            return true;
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