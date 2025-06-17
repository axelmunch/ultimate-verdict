import { Link } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";

interface RoundListItemProps {
  id: number;
  roundNumber: number;
}

function RoundListItem({ id, roundNumber }: RoundListItemProps) {
  return (
    <>
      <Link component={RouterLink} to={`round/${id}`} underline="hover">
        Round #{roundNumber}
      </Link>
    </>
  );
}

export default RoundListItem;
