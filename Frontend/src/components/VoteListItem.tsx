import { Link } from "@mui/material";
import { Link as RouterLink } from 'react-router-dom';

interface VoteListItemProps {
  id: number;
}

function VoteListItem({ id }: VoteListItemProps) {
  return (
    <>
      <Link component={RouterLink} to={`/vote/${id}`} underline="hover">
        Vote #{id}
      </Link>
    </>
  );
}

export default VoteListItem;
