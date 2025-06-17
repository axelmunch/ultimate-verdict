import { Outlet, useNavigate, useParams } from "react-router-dom";
import { useApi } from "../ApiContext";
import { useEffect, useState } from "react";
import type { Vote as VoteType } from "../types";
import CircularProgress from "@mui/material/CircularProgress";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import IconButton from "@mui/material/IconButton";
import DialogContent from "@mui/material/DialogContent";
import Typography from "@mui/material/Typography";
import CloseIcon from "@mui/icons-material/Close";
import Divider from "@mui/material/Divider";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemText from "@mui/material/ListItemText";
import InfoOutlineIcon from "@mui/icons-material/InfoOutline";
import Box from "@mui/material/Box";
import ListItemButton from "@mui/material/ListItemButton";

function Vote() {
  const { voteId: voteIdParam, roundId: roundIdParam } = useParams();
  const voteId = Number(voteIdParam);
  const roundId = Number(roundIdParam);

  const [vote, setVote] = useState<VoteType | null>(null);

  const [viewVoteDetails, setViewVoteDetails] = useState(false);

  const navigate = useNavigate();

  const { getVote } = useApi();

  useEffect(() => {
    getVote(voteId)
      .then(setVote)
      .catch(() => navigate("/"));
  }, [getVote, navigate, voteId]);

  return vote === null ? (
    <CircularProgress />
  ) : (
    <>
      <Box display="flex" alignItems="center">
        <Typography variant="h5">
          {vote.name} {vote.id}
        </Typography>

        <IconButton onClick={() => setViewVoteDetails(true)} color="primary">
          <InfoOutlineIcon />
        </IconButton>
      </Box>

      <Dialog onClose={() => setViewVoteDetails(false)} open={viewVoteDetails}>
        <DialogTitle
          display="flex"
          alignItems="center"
          justifyContent="space-between"
        >
          Détails sur le vote
          <IconButton onClick={() => setViewVoteDetails(false)}>
            <CloseIcon />
          </IconButton>
        </DialogTitle>
        <Divider />
        <DialogContent>
          <Typography variant="h6" gutterBottom>
            {vote.name}
          </Typography>
          <Typography gutterBottom>{vote.description}</Typography>
        </DialogContent>

        <Divider />

        <DialogContent>
          <List subheader={<Typography>Options :</Typography>} dense>
            {vote.options.map((option) => (
              <ListItem key={option.id}>
                <ListItemText primary={option.name} />
              </ListItem>
            ))}
          </List>

          <Typography gutterBottom>Visibilité : {vote.visibility}</Typography>
          <Typography gutterBottom>Type : {vote.type}</Typography>
          <Typography gutterBottom>
            Nombre de tours : {vote.nbRounds}
          </Typography>
          <Typography gutterBottom>
            Nombre de gagnants par tour (respectivement) :{" "}
            {vote.winnersByRound.join(", ")}
          </Typography>
          <Typography gutterBottom>
            Conditions de victoire : {vote.victoryCondition}
          </Typography>
          <Typography gutterBottom>
            Rejeu en cas d'égalité : {vote.replayOnDraw ? "oui" : "non"}
          </Typography>
        </DialogContent>
      </Dialog>

      <List>
        {vote.rounds.map((round, index) => (
          <ListItemButton
            key={round.id}
            onClick={() => navigate(`round/${round.id}`)}
            selected={round.id === roundId}
          >
            <ListItemText>
              Round #{index + 1} {round.id}
            </ListItemText>
          </ListItemButton>
        ))}
      </List>

      <Outlet />
    </>
  );
}

export default Vote;
