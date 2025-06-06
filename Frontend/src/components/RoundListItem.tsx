import { Link } from "@mui/material";
import { Link as RouterLink } from 'react-router-dom';

interface RoundListItemProps {
  id: number;
}

function RoundListItem({ id }: RoundListItemProps) {
  return (
    <>
      <Link component={RouterLink} to={`round/${id}`} underline="hover">
        Round #{id}
      </Link>
    </>
  );
}

export default RoundListItem;
