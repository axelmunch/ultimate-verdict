import { type Result as ResultType } from "../types";
import Typography from "@mui/material/Typography";

interface ResultProps {
  result: ResultType;
}

function Result({ result }: ResultProps) {
  return (
    <>
      <Typography variant="h6">Résultat</Typography>
      <Typography>{result.res}</Typography>
      {result.options.map((option, index) => (
        <Typography key={option.id}>
          {option.name} : {result.scores[index]}
        </Typography>
      ))}
    </>
  );
}

export default Result;
