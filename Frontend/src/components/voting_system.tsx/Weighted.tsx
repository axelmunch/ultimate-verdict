import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import { useEffect, useMemo, useState } from "react";
import Button from "@mui/material/Button";
import type { Option } from "../../types";

interface WeightedProps {
  options: Option[];
  maxPoints: number;
  setCanSubmit: (canSubmit: boolean) => void;
}

interface OptionScore {
  id: number;
  score: number;
}

function Weighted({ options, maxPoints, setCanSubmit }: WeightedProps) {
  const initialPoints = useMemo<OptionScore[]>(
    () =>
      options.map((option) => ({
        id: option.id,
        score: 0,
      })),
    [options]
  );
  const [points, setPoints] = useState<OptionScore[]>(initialPoints);
  const [totalAllocated, setTotalAllocated] = useState<number>(0);

  useEffect(() => {
    const allocated = points.reduce((sum, option) => sum + option.score, 0);
    setTotalAllocated(allocated);
    setCanSubmit(allocated === maxPoints);
  }, [points, maxPoints, setCanSubmit]);

  const handleChange = (id: number, valueStr: string) => {
    let value = parseInt(valueStr, 10);
    if (isNaN(value)) value = 0;
    if (value < 0) value = 0;

    const currentValue = points.find((point) => point.id === id)?.score || 0;
    const maxAllowed = maxPoints - (totalAllocated - currentValue);

    if (value > maxAllowed) value = maxAllowed;

    setPoints((prevPoints) =>
      prevPoints.map((point) =>
        point.id === id ? { ...point, score: value } : point
      )
    );
  };

  return (
    <Paper>
      <Typography variant="h6" gutterBottom>
        Weighted {maxPoints} points
      </Typography>
      <Typography variant="body1">
        {maxPoints - totalAllocated} / {maxPoints}
      </Typography>
      <Typography variant="body1">Instructions</Typography>
      <Button onClick={() => setPoints(initialPoints)}>Reset</Button>
      {points.map((point) => (
        <Box
          key={point.id}
          sx={{ mb: 2, display: "flex", alignItems: "center", gap: 2 }}
        >
          <Typography>
            {options.find((option) => option.id === point.id)?.name || ""}:
          </Typography>
          <TextField
            type="number"
            value={point.score}
            onChange={(e) => handleChange(point.id, e.target.value)}
          />
          <Typography>pts</Typography>
        </Box>
      ))}
    </Paper>
  );
}

export default Weighted;
