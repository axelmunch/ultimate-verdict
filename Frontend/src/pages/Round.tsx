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
import type { Decision, Vote } from "../types";
import CircularProgress from "@mui/material/CircularProgress";
import { useApi } from "../ApiContext";
import type { Round as RoundType } from "../types";

function Round() {
  const { roundId: roundIdParam, voteId: voteIdParam } = useParams();
  const roundId = Number(roundIdParam);
  const voteId = Number(voteIdParam);
  const [canSubmit, setCanSubmit] = useState(false);
  const [confirmVote, setConfirmVote] = useState(false);
  const [loading, setLoading] = useState(false);

  const [decisions, setDecisions] = useState<Decision[]>([]);

  const navigate = useNavigate();

  const { getVote, submitDecision } = useApi();

  const [round, setRound] = useState<RoundType | null>(null);

  useEffect(() => {
    getVote(voteId)
      .then((vote: Vote) => {
        const selectedRound = vote.rounds.find((r) => r.id === roundId);
        if (!selectedRound) {
          return navigate(`/vote/${voteId}`);
        }
        setRound(selectedRound);
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
      .then(() => navigate(`/vote/${voteId}`))
      .catch(console.error)
      .finally(() => {
        setLoading(false);
        setConfirmVote(false);
      });
  };

  return round === null ? (
    <CircularProgress />
  ) : (
    <>
      <h5>Round {round.id}</h5>
      <p>Round in progress</p>
      <p>Voting</p>
      <p>You have voted</p>
      <p>-</p>
      <p>Round finished</p>
      <p>Round result</p>
      <p>Results are hidden until round's end</p>
      {round.result === null && (
        <>
          <SingleChoice
            options={round.options}
            setCanSubmit={setCanSubmit}
            setDecisions={setDecisions}
          />
          <Ranking
            options={round.options}
            setCanSubmit={setCanSubmit}
            setDecisions={setDecisions}
          />
          <Weighted
            options={round.options}
            setCanSubmit={setCanSubmit}
            setDecisions={setDecisions}
          />
          <Button disabled={!canSubmit} onClick={() => setConfirmVote(true)}>
            Submit
          </Button>
        </>
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
