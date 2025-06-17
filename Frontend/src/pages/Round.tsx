import { useNavigate, useParams } from "react-router-dom";
import SingleChoice from "../components/voting_system.tsx/SingleChoice";
import Ranking from "../components/voting_system.tsx/Ranking";
import Weighted from "../components/voting_system.tsx/Weighted";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogActions from "@mui/material/DialogActions";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import { useEffect, useState } from "react";
import { type Decision, type Vote, type VotingSystemProps } from "../types";
import CircularProgress from "@mui/material/CircularProgress";
import { useApi } from "../ApiContext";
import type { Round as RoundType } from "../types";
import Result from "../components/Result";
import { useTime } from "../TimeContext";
import Typography from "@mui/material/Typography";
import RefreshIcon from "@mui/icons-material/Refresh";

function Round() {
  const { roundId: roundIdParam, voteId: voteIdParam } = useParams();
  const roundId = Number(roundIdParam);
  const voteId = Number(voteIdParam);
  const [canSubmit, setCanSubmit] = useState(false);
  const [confirmVote, setConfirmVote] = useState(false);
  const [loading, setLoading] = useState(false);

  const [decisions, setDecisions] = useState<Decision[]>([]);

  const navigate = useNavigate();

  const { currentTime } = useTime();

  const { getVote, submitDecision } = useApi();

  const [vote, setVote] = useState<Vote | null>(null);
  const [round, setRound] = useState<RoundType | null>(null);
  const [votingSystemProps, setVotingSystemProps] = useState<VotingSystemProps>(
    { options: [], setCanSubmit, setDecisions },
  );

  useEffect(() => {
    getVote(voteId)
      .then((selectedVote: Vote) => {
        setVote(selectedVote);
        const selectedRound = selectedVote.rounds.find((r) => r.id === roundId);
        if (!selectedRound) {
          return navigate(`/vote/${voteId}`);
        }
        setRound(selectedRound);

        setVotingSystemProps({
          options: selectedRound.options,
          setCanSubmit,
          setDecisions,
        });
      })
      .catch(() => navigate("/"));
  }, [getVote, navigate, roundId, voteId]);

  useEffect(() => {
    console.log(decisions);
  }, [decisions]);

  const closeConfirmVote = () => {
    if (!loading) {
      setConfirmVote(false);
    }
  };

  const submitConfirmVote = () => {
    setLoading(true);

    submitDecision(roundId, decisions)
      .then(() => {
        navigate(`/vote/${voteId}`);
        setConfirmVote(false);
      })
      .catch(console.error)
      .finally(() => {
        setLoading(false);
      });
  };

  return round === null ? (
    <CircularProgress />
  ) : (
    <>
      {round.result === null ? (
        round.endTime < currentTime ? (
          <>
            <Typography>
              Ce tour est terminé. Cliquez pour rafraichir les informations
            </Typography>
            <Button
              startIcon={<RefreshIcon />}
              onClick={() => navigate(0)}
              variant="contained"
            >
              Actualiser
            </Button>
          </>
        ) : (
          <>
            {(() => {
              switch (vote?.type) {
                case "plural":
                case "elo":
                  return <SingleChoice {...votingSystemProps} />;
                case "ranked":
                  return <Ranking {...votingSystemProps} />;
                case "weighted":
                  return <Weighted {...votingSystemProps} />;
                default:
                  return null;
              }
            })()}
            <Button disabled={!canSubmit} onClick={() => setConfirmVote(true)}>
              Submit
            </Button>
          </>
        )
      ) : (
        <Result result={round.result} />
      )}

      <Dialog fullWidth open={confirmVote} onClose={closeConfirmVote}>
        <DialogTitle>Confirme le vote ?</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Ci-dessous, votre attributon en nombre de voies :
          </DialogContentText>
          <List>
            {decisions.map((decision, index) => (
              <ListItem key={index}>
                <DialogContentText>
                  {round.options.find((option) => option.id === decision.id)
                    ?.name || ""}
                  : {decision.score}
                </DialogContentText>
              </ListItem>
            ))}
          </List>
        </DialogContent>
        <DialogActions>
          {loading ? (
            <CircularProgress />
          ) : (
            <>
              <Button onClick={closeConfirmVote} variant="outlined">
                Annuler
              </Button>
              <Button onClick={submitConfirmVote} variant="contained">
                Confirmer
              </Button>
            </>
          )}
        </DialogActions>
      </Dialog>
    </>
  );
}

export default Round;
