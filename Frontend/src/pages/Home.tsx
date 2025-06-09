import List from "@mui/material/List";
import VoteListItem from "../components/VoteListItem";
import ListItem from "@mui/material/ListItem";
import { useApi } from "../ApiContext";
import { useEffect, useState } from "react";
import type { Vote } from "../types";

function Home() {
  const [votes, setVotes] = useState<Vote[]>([]);

  const { getVotes } = useApi();

  useEffect(() => {
    getVotes().then(setVotes).catch(console.error);
  }, [getVotes]);

  return (
    <>
      <List>
        {votes.map((vote) => {
          return (
            <ListItem key={vote.id}>
              <VoteListItem vote={vote} />
            </ListItem>
          );
        })}
      </List>
    </>
  );
}

export default Home;
