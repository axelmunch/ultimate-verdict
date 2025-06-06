import { useParams } from 'react-router-dom';
import SingleChoice from '../components/voting_system.tsx/SingleChoice';
import Ranking from '../components/voting_system.tsx/Ranking';
import Weighted from '../components/voting_system.tsx/Weighted';
import Button from '@mui/material/Button';
import { useState } from 'react';

const options = [
  "Option A",
  "Option B",
  "Option C",
  "Option D",
];

function Round() {
  const { roundId } = useParams();
  const [canSubmit, setCanSubmit] = useState(false);

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
      <SingleChoice options={options} setCanSubmit={setCanSubmit} />
      <Ranking options={options} setCanSubmit={setCanSubmit} />
      <Weighted options={options} maxPoints={options.length * (options.length + 1) / 2} setCanSubmit={setCanSubmit} />
      <Button disabled={!canSubmit}>Submit</Button>
    </>
  );
}

export default Round;
