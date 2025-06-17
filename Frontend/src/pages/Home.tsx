import VoteListItem from "../components/VoteListItem";
import { useApi } from "../ApiContext";
import { useEffect, useState } from "react";
import { type Vote } from "../types";
import Fab from "@mui/material/Fab";
import AddIcon from "@mui/icons-material/Add";
import CreateVote from "../components/CreateVote";
import Box from "@mui/material/Box";

function Home() {
  const [votes, setVotes] = useState<Vote[]>([]);

  const [createVoteOpen, setCreateVoteOpen] = useState(false);

  const { getVotes } = useApi();

  useEffect(() => {
    getVotes().then(setVotes).catch(console.error);
  }, [getVotes]);

  return (
    <>
      <Box display="flex" flexWrap="wrap" gap={2} data-votes>
        {votes.map((vote) => {
          return <VoteListItem key={vote.id} vote={vote} />;
        })}
      </Box>

      <Fab
        onClick={() => setCreateVoteOpen(true)}
        variant="extended"
        color="primary"
        data-create-vote
      >
        <AddIcon sx={{ mr: 1 }} />
        Vote
      </Fab>

      <CreateVote
        open={createVoteOpen}
        close={() => setCreateVoteOpen(false)}
      />
    </>
  );
}

export default Home;
