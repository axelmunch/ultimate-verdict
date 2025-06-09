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
import type { Decision, Option } from "../types";
import CircularProgress from "@mui/material/CircularProgress";
import { useApi } from "../ApiContext";

const options: Option[] = [
  { id: 10, name: "Option A" },
  { id: 3, name: "Option B" },
  { id: 1, name: "Option C" },
  { id: 100, name: "Option D" },
  { id: 6, name: "Option E" },
];

function Round() {
  const { roundId: roundIdParam, voteId: voteIdParam } = useParams();
  const roundId = Number(roundIdParam);
  const voteId = Number(voteIdParam);
  const [canSubmit, setCanSubmit] = useState(false);
  const [confirmVote, setConfirmVote] = useState(false);
  const [loading, setLoading] = useState(false);

  const [decisions, setDecisions] = useState<Decision[]>([]);

  const navigate = useNavigate();

  const { submitDecision } = useApi();

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

  return (
    <>
      <h5>Round {roundId}</h5>
      <p>Round in progress</p>
      <p>Voting</p>
      <p>You have voted</p>
      <p>-</p>
      <p>Round finished</p>
      <p>Round result</p>
      <p>Results are hidden until round's end</p>
      <SingleChoice
        options={options}
        setCanSubmit={setCanSubmit}
        setDecisions={setDecisions}
      />
      <Ranking
        options={options}
        setCanSubmit={setCanSubmit}
        setDecisions={setDecisions}
      />
      <Weighted
        options={options}
        setCanSubmit={setCanSubmit}
        setDecisions={setDecisions}
      />
      <Button disabled={!canSubmit} onClick={() => setConfirmVote(true)}>
        Submit
      </Button>

      <Dialog fullWidth open={confirmVote} onClose={closeConfirmVote}>
        <DialogTitle>Confirme le vote ?</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Ci-dessous, votre attributon des points :
          </DialogContentText>
          <List>
            {decisions.map((decision, index) => (
              <ListItem key={index}>
                <DialogContentText>
                  {options.find((option) => option.id === decision.id)?.name ||
                    ""}
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
                Retour
              </Button>
              <Button onClick={submitConfirmVote} variant="contained">
                Envoyer
              </Button>
            </>
          )}
        </DialogActions>
      </Dialog>
    </>
  );
}

export default Round;
