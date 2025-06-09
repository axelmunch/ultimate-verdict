import { Link } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";
import type { Vote } from "../types";

interface VoteListItemProps {
  vote: Vote;
}

function VoteListItem({ vote }: VoteListItemProps) {
  return (
    <>
      <Link component={RouterLink} to={`/vote/${vote.id}`} underline="hover">
        {vote.name}
        {vote.description}#{vote.id}
      </Link>
    </>
  );
}

export default VoteListItem;
