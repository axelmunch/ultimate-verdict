import { Outlet, useParams } from "react-router-dom";
import RoundListItem from "../components/RoundListItem";

function Vote() {
  const { voteId } = useParams();

  return (
    <>
      <h4>Vote {voteId}</h4>
      <p>Vote in progress</p>
      <p>-</p>
      <p>Vote finished</p>
      <p>Vote result</p>
      <p>Results are hidden until vote's end</p>

      <ul>
        {[8, 9, 10].map((item) => {
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
