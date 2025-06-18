import Button from "@mui/material/Button";
import FormControl from "@mui/material/FormControl";
import FormControlLabel from "@mui/material/FormControlLabel";
import Paper from "@mui/material/Paper";
import Radio from "@mui/material/Radio";
import RadioGroup from "@mui/material/RadioGroup";
import Typography from "@mui/material/Typography";
import { useEffect, useState } from "react";
import type { VotingSystemProps } from "../../types";

function SingleChoice({
  options,
  setCanSubmit,
  setDecisions,
}: VotingSystemProps) {
  const [selectedValue, setSelectedValue] = useState<number | null>(null);

  useEffect(
    () => setCanSubmit(selectedValue !== null),
    [selectedValue, setCanSubmit],
  );

  useEffect(() => {
    if (selectedValue !== null) {
      setDecisions([
        {
          id: selectedValue,
          score: 1,
        },
      ]);
    } else {
      setDecisions([]);
    }
  }, [selectedValue, setDecisions]);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSelectedValue(Number((event.target as HTMLInputElement).value));
  };

  return (
    <Paper data-component>
      <Typography variant="h6" gutterBottom>
        Single Choice
      </Typography>
      <Typography variant="body1">Instructions</Typography>
      <Button data-reset onClick={() => setSelectedValue(null)}>
        Reset
      </Button>
      <FormControl>
        <RadioGroup value={selectedValue} onChange={handleChange}>
          {options.map((option) => (
            <FormControlLabel
              data-option={option.id}
              key={option.id}
              value={option.id}
              control={<Radio />}
              label={option.name}
              data-input
            />
          ))}
        </RadioGroup>
      </FormControl>
    </Paper>
  );
}

export default SingleChoice;
