import { type Result as ResultType } from "../types";
import Typography from "@mui/material/Typography";

interface ResultProps {
  result: ResultType;
}

function Result({ result }: ResultProps) {
  return (
    <>
      <Typography>{result.id}</Typography>
      <Typography>{result.res}</Typography>
      {result.options.map((option, index) => (
        <Typography>
          {option.name} : {result.scores[index]}
        </Typography>
      ))}
    </>
  );
}

export default Result;
