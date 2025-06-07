import { useParams } from "react-router-dom";
import SingleChoice from "../components/voting_system.tsx/SingleChoice";
import Ranking from "../components/voting_system.tsx/Ranking";
import Weighted from "../components/voting_system.tsx/Weighted";
import Button from "@mui/material/Button";
import { useEffect, useState } from "react";
import type { Decision, Option } from "../types";

const options: Option[] = [
  { id: 10, name: "Option A" },
  { id: 3, name: "Option B" },
  { id: 1, name: "Option C" },
  { id: 100, name: "Option D" },
  { id: 6, name: "Option E" },
];

function Round() {
  const { roundId } = useParams();
  const [canSubmit, setCanSubmit] = useState(false);

  const [decisions, setDecisions] = useState<Decision[]>([]);

  useEffect(() => {
    console.log(decisions);
  }, [decisions]);

  return (
    <>
      <h5>Round {roundId}</h5>
      <p>Round in progress</p>
      <p>Voting</p>
      <p>You have voted</p>
      <p>-</p>
      <p>Round finished</p>
      <p>Round result</p>
      <p>Results are hidden until round's end</p>
      <SingleChoice
        options={options}
        setCanSubmit={setCanSubmit}
        setDecisions={setDecisions}
      />
      <Ranking
        options={options}
        setCanSubmit={setCanSubmit}
        setDecisions={setDecisions}
      />
      <Weighted
        options={options}
        maxPoints={(options.length * (options.length + 1)) / 2}
        setCanSubmit={setCanSubmit}
        setDecisions={setDecisions}
      />
      <Button disabled={!canSubmit}>Submit</Button>
    </>
  );
}

export default Round;
