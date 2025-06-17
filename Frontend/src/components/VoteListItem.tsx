import { Link } from "react-router-dom";
import type { Vote } from "../types";
import Card from "@mui/material/Card";
import CardActionArea from "@mui/material/CardActionArea";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";

interface VoteListItemProps {
  vote: Vote;
}

function VoteListItem({ vote }: VoteListItemProps) {
  return (
    <>
      <Card
        component={Link}
        to={`/vote/${vote.id}`}
        sx={{ textDecoration: "none", height: "100%" }}
      >
        <CardActionArea>
          <CardContent>
            <Typography variant="h5">{vote.name}</Typography>
            {vote.description.length > 0 ? (
              <Typography variant="body2" color="text.secondary">
                {vote.description}
              </Typography>
            ) : (
              <Typography
                variant="body2"
                color="text.secondary"
                sx={{ fontStyle: "italic" }}
              >
                Pas de description
              </Typography>
            )}
          </CardContent>
        </CardActionArea>
      </Card>
    </>
  );
}

export default VoteListItem;
