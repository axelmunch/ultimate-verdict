import Button from "@mui/material/Button";
import FormControl from "@mui/material/FormControl";
import FormControlLabel from "@mui/material/FormControlLabel";
import Paper from "@mui/material/Paper";
import Radio from "@mui/material/Radio";
import RadioGroup from "@mui/material/RadioGroup";
import Typography from "@mui/material/Typography";
import { useEffect, useState } from "react";
import type { Option } from "../../types";

interface SingleChoiceProps {
  options: Option[];
  setCanSubmit: (canSubmit: boolean) => void;
}

function SingleChoice({ options, setCanSubmit }: SingleChoiceProps) {
  const [selectedValue, setSelectedValue] = useState<number | null>(null);

  useEffect(() => {
    setCanSubmit(selectedValue !== null);
  }, [selectedValue, setCanSubmit]);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSelectedValue(Number((event.target as HTMLInputElement).value));
  };

  return (
    <Paper>
      <Typography variant="h6" gutterBottom>
        Single Choice
      </Typography>
      <Typography variant="body1">Instructions</Typography>
      <Button onClick={() => setSelectedValue(null)}>Reset</Button>
      <FormControl>
        <RadioGroup value={selectedValue} onChange={handleChange}>
          {options.map((option) => (
            <FormControlLabel
              key={option.id}
              value={option.id}
              control={<Radio />}
              label={option.name}
            />
          ))}
        </RadioGroup>
      </FormControl>
    </Paper>
  );
}

export default SingleChoice;
