import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import { useEffect, useState } from "react";
import Button from "@mui/material/Button";

interface WeightedProps {
  options: string[];
  maxPoints: number;
  setCanSubmit: (canSubmit: boolean) => void;
}

function Weighted({ options, maxPoints, setCanSubmit }: WeightedProps) {
  const initialPoints = options.reduce<Record<string, number>>((acc, option) => {
    acc[option] = 0;
    return acc;
  }, {});
  const [points, setPoints] = useState<Record<string, number>>(initialPoints);
  const [totalAllocated, setTotalAllocated] = useState<number>(0);

  useEffect(() => {
    const allocated = Object.values(points).reduce(
      (sum, val) => sum + val,
      0
    );
    setTotalAllocated(allocated);
    setCanSubmit(allocated === maxPoints);
  }, [maxPoints, points, setCanSubmit]);

  const handleChange = (option: string, valueStr: string) => {
    let value = parseInt(valueStr, 10);
    if (isNaN(value)) value = 0;
    if (value < 0) value = 0;

    const currentValue = points[option];
    const maxAllowed = maxPoints - (totalAllocated - currentValue);

    if (value > maxAllowed) value = maxAllowed;

    setPoints((prev) => ({ ...prev, [option]: value }));
  };

  return (
    <Paper>
      <Typography variant="h6" gutterBottom>
        Weighted {maxPoints} points
      </Typography>
      <Typography variant="body1">
        {maxPoints - totalAllocated} / {maxPoints}
      </Typography>
      <Typography variant="body1">
        Instructions
      </Typography>
      <Button onClick={() => setPoints(initialPoints)}>Reset</Button>
      {options.map((option) => (
        <Box key={option} sx={{ mb: 2, display: "flex", alignItems: "center", gap: 2 }}>
          <Typography sx={{ width: 100 }}>
            {option}:
          </Typography>
          <TextField
            type="number"
            value={points[option]}
            onChange={(e) => handleChange(option, e.target.value)}
          />
          <Typography>pts</Typography>
        </Box>
      ))}
    </Paper>
  );
}

export default Weighted;
