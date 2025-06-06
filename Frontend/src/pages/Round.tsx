import { useParams } from 'react-router-dom';

function Round() {
  const { roundId } = useParams();

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
    </>
  );
}

export default Round;
