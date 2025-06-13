import { Outlet, useNavigate, useParams } from "react-router-dom";
import RoundListItem from "../components/RoundListItem";
import { useApi } from "../ApiContext";
import { useEffect, useState } from "react";
import type { Vote as VoteType } from "../types";
import CircularProgress from "@mui/material/CircularProgress";

function Vote() {
  const { voteId: voteIdParam } = useParams();
  const voteId = Number(voteIdParam);

  const [vote, setVote] = useState<VoteType | null>(null);

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
      <h4>
        {vote.name} {vote.id}
      </h4>
      <p>Vote in progress</p>
      <p>-</p>
      <p>Vote finished</p>
      <p>Vote result</p>
      <p>Results are hidden until vote's end</p>

      <ul>
        {vote.rounds
          .map((r) => r.id)
          .map((item) => {
            return (
              <li key={item}>
                <RoundListItem id={item} />
              </li>
            );
          })}
      </ul>

      <Outlet />
    </>
  );
}

export default Vote;
