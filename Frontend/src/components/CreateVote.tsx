import dayjs from "dayjs";
import { useState } from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import IconButton from "@mui/material/IconButton";
import Divider from "@mui/material/Divider";
import TextField from "@mui/material/TextField";
import Switch from "@mui/material/Switch";
import FormControlLabel from "@mui/material/FormControlLabel";
import FormControl from "@mui/material/FormControl";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";
import InputLabel from "@mui/material/InputLabel";
import { DemoContainer } from "@mui/x-date-pickers/internals/demo";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DateTimePicker } from "@mui/x-date-pickers/DateTimePicker";
import Box from "@mui/material/Box";
import AddIcon from "@mui/icons-material/Add";
import CloseIcon from "@mui/icons-material/Close";
import DeleteIcon from "@mui/icons-material/Delete";
import Alert from "@mui/material/Alert";
import { TransitionGroup } from "react-transition-group";
import Collapse from "@mui/material/Collapse";
import { type VoteInput } from "../types";
import { useApi } from "../ApiContext";
import CircularProgress from "@mui/material/CircularProgress";
import { useNavigate } from "react-router-dom";

interface CreateVoteProps {
  open: boolean;
  close: () => void;
}

function CreateVote({ open, close }: CreateVoteProps) {
  const [vote, setVote] = useState<VoteInput>({
    name: "",
    description: "",
    visibility: "public",
    type: "plural",
    nbRounds: 1,
    winnersByRound: [1],
    victoryCondition: "majority",
    replayOnDraw: false,
    options: ["", ""],
    startDate: dayjs().unix() * 1000,
    roundDuration: 1000 * 60 * 60,
  });
  const [timeUnit, setTimeUnit] = useState<"m" | "h" | "d">("h");

  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  const handleChange = (field: string, value: unknown) => {
    setVote({ ...vote, [field]: value });
  };

  const handleWinnersByRoundChange = (index: number, value: number) => {
    if (isNaN(value) || value < 1) return;

    if (index > 0 && value > vote.winnersByRound[index - 1]) {
      return;
    }

    if (value > vote.options.length) {
      value = vote.options.length;
    }

    const newWinners = [...vote.winnersByRound];
    newWinners[index] = value;

    for (let i = index + 1; i < newWinners.length; i++) {
      if (newWinners[i] > value) {
        newWinners[i] = value;
      }
    }

    setVote({ ...vote, winnersByRound: newWinners });
  };

  const { createVote } = useApi();

  return (
    <Dialog
      open={open}
      fullWidth
      slotProps={{
        paper: {
          component: "form",
          onSubmit: (event: React.FormEvent<HTMLFormElement>) => {
            event.preventDefault();

            setLoading(true);
            console.log(vote);
            createVote(vote)
              .then((id) => console.log("Vote created with ID:", id))
              .then(() => {
                close();
                navigate(0);
              })
              .catch((error) => {
                console.error("Error creating vote:", error);
              })
              .finally(() => {
                setLoading(false);
              });
          },
        },
      }}
    >
      <DialogTitle
        display="flex"
        alignItems="center"
        justifyContent="space-between"
      >
        Créer un vote
        <IconButton onClick={close}>
          <CloseIcon />
        </IconButton>
      </DialogTitle>
      <Divider />

      <DialogContent>
        <TextField
          label="Nom"
          value={vote.name}
          onChange={(event) => handleChange("name", event.target.value)}
          fullWidth
          required
          margin="normal"
          data-name
        />

        <TextField
          label="Description"
          value={vote.description}
          onChange={(event) => handleChange("description", event.target.value)}
          fullWidth
          multiline
          rows={2}
          margin="normal"
          data-description
        />

        <FormControl fullWidth margin="normal">
          <InputLabel>Visibilité</InputLabel>
          <Select
            value={vote.visibility}
            onChange={(event) => handleChange("visibility", event.target.value)}
            label="Visibilité"
            data-visibility-select
          >
            <MenuItem value="public" data-visibility>
              Publique
            </MenuItem>
            <MenuItem value="private" data-visibility>
              Privée
            </MenuItem>
          </Select>
        </FormControl>

        <Divider sx={{ marginY: 2 }} />

        <FormControl fullWidth margin="normal">
          <InputLabel>Type</InputLabel>
          <Select
            value={vote.type}
            onChange={(event) => {
              if (event.target.value === "elo") {
                setVote((prev) => ({
                  ...prev,
                  type: event.target.value,
                  nbRounds: 1,
                  winnersByRound: [1],
                  victoryCondition: "none",
                  replayOnDraw: false,
                }));

                return;
              }
              handleChange("type", event.target.value);
            }}
            label="Type"
            data-type-select
          >
            <MenuItem value="plural" data-type>
              Plural
            </MenuItem>
            <MenuItem value="ranked" data-type>
              Classement
            </MenuItem>
            <MenuItem value="weighted" data-type>
              Pondéré
            </MenuItem>
            {/* <MenuItem value="elo" data-type>
              ELO
            </MenuItem> */}
          </Select>
        </FormControl>

        <FormControl fullWidth margin="normal">
          <InputLabel>Condition de victoire</InputLabel>
          <Select
            value={vote.victoryCondition}
            onChange={(event) => {
              if (event.target.value === "last man standing") {
                setVote((prev) => ({
                  ...prev,
                  victoryCondition: event.target.value,
                  nbRounds: 1,
                  winnersByRound: [1],
                }));

                return;
              } else if (event.target.value === "none") {
                setVote((prev) => ({
                  ...prev,
                  victoryCondition: event.target.value,
                  replayOnDraw: false,
                }));

                return;
              }

              handleChange("victoryCondition", event.target.value);
            }}
            label="Condition de victoire"
            disabled={vote.type === "elo"}
            data-victory-condition-select
          >
            <MenuItem value="none" data-victory-condition>
              Aucune
            </MenuItem>
            <MenuItem value="majority" data-victory-condition>
              Majorité
            </MenuItem>
            <MenuItem value="absolute majority" data-victory-condition>
              Majorité absolue
            </MenuItem>
            <MenuItem value="2/3 majority" data-victory-condition>
              Majorité 2/3
            </MenuItem>
            <MenuItem value="last man standing" data-victory-condition>
              Elimination
            </MenuItem>
          </Select>
        </FormControl>

        <FormControlLabel
          control={
            <Switch
              checked={vote.replayOnDraw}
              onChange={(event) =>
                handleChange("replayOnDraw", event.target.checked)
              }
              disabled={vote.type === "elo" || vote.victoryCondition === "none"}
              data-replay-on-draw
            />
          }
          label="Rejeu en cas d'égalité"
        />

        <Divider sx={{ marginY: 2 }} />

        {vote.type === "elo" && vote.options.length > 4 ? (
          <Alert severity="warning">
            La création d'un vote ELO avec {vote.options.length} options
            entrainera la création de{" "}
            {(vote.options.length * (vote.options.length - 1)) / 2} tours
          </Alert>
        ) : null}

        <List>
          <TransitionGroup component={null}>
            {vote.options.map((option, index) => (
              <Collapse key={index} timeout={200} sx={{ flexShrink: 0 }}>
                <ListItem sx={{ paddingX: 0 }}>
                  <TextField
                    label={`Option ${index + 1}`}
                    value={option}
                    onChange={(event) => {
                      const newOptions = [...vote.options];
                      newOptions[index] = event.target.value;
                      handleChange("options", newOptions);
                    }}
                    fullWidth
                    required
                    data-option
                  />
                  <IconButton
                    onClick={() => {
                      const newOptions = [...vote.options];
                      newOptions.splice(index, 1);

                      const newWinnersByRound = vote.winnersByRound.map(
                        (val) =>
                          val > newOptions.length ? newOptions.length : val,
                      );

                      setVote((prev) => ({
                        ...prev,
                        winnersByRound: newWinnersByRound,
                        options: newOptions,
                      }));
                    }}
                    disabled={vote.options.length <= 2}
                    data-option-delete
                  >
                    <DeleteIcon />
                  </IconButton>
                </ListItem>
              </Collapse>
            ))}
          </TransitionGroup>
        </List>

        <Button
          onClick={() => {
            handleChange("options", [...vote.options, ""]);
          }}
          startIcon={<AddIcon />}
          variant="outlined"
          data-option-add
        >
          Option
        </Button>

        <Divider sx={{ marginY: 2 }} />

        <TextField
          type="number"
          label="Nombre de tours"
          value={vote.nbRounds}
          onChange={(event) => {
            const value = Math.max(
              1,
              Math.min(5, parseInt(event.target.value) || 1),
            );

            const newWinnersByRound = vote.winnersByRound.slice(0, value);
            while (newWinnersByRound.length < value) {
              newWinnersByRound.push(1);
            }
            setVote({
              ...vote,
              nbRounds: value,
              winnersByRound: newWinnersByRound,
            });
          }}
          disabled={
            vote.type === "elo" || vote.victoryCondition === "last man standing"
          }
          fullWidth
          margin="normal"
          data-winners-by-round-count
        />

        <Box
          display="flex"
          flexWrap="wrap"
          justifyContent="space-around"
          gap={2}
          marginTop={2}
        >
          <TransitionGroup component={null}>
            {vote.winnersByRound.map((val, index) => (
              <Collapse key={index} timeout={200} sx={{ flexShrink: 0 }}>
                <TextField
                  type="number"
                  label={`Gagnants tour ${index + 1}`}
                  value={val}
                  onChange={(event) =>
                    handleWinnersByRoundChange(
                      index,
                      parseInt(event.target.value) || 0,
                    )
                  }
                  disabled={
                    vote.type === "elo" ||
                    vote.victoryCondition === "last man standing"
                  }
                  data-winners-by-round
                />
              </Collapse>
            ))}
          </TransitionGroup>
        </Box>

        <Divider sx={{ marginY: 2 }} />

        <LocalizationProvider dateAdapter={AdapterDayjs}>
          <DemoContainer components={["DateTimePicker", "DateTimePicker"]}>
            <DateTimePicker
              label="Date de début"
              value={dayjs(vote.startDate)}
              onChange={(value) => {
                if (value) {
                  handleChange("startDate", value.unix() * 1000);
                }
              }}
              data-start-date
            />
          </DemoContainer>
        </LocalizationProvider>

        <Box display="flex" gap={2}>
          <Box width="100%">
            <TextField
              type="number"
              label={`Durée des tours (${{ m: "minutes", h: "heures", d: "jours" }[timeUnit]})`}
              value={
                vote.roundDuration /
                (1000 * 60 * { m: 1, h: 60, d: 1440 }[timeUnit])
              }
              onChange={(event) => {
                const value = Math.max(1, parseInt(event.target.value) || 0);
                handleChange(
                  "roundDuration",
                  value * 1000 * 60 * { m: 1, h: 60, d: 1440 }[timeUnit],
                );
              }}
              fullWidth
              margin="normal"
              data-round-duration
            />
          </Box>

          <Box width="100%">
            <FormControl fullWidth margin="normal">
              <InputLabel>Unité</InputLabel>
              <Select
                value={timeUnit}
                onChange={(event) => {
                  const newTimeMeasure = event.target.value;
                  const oldTimeMeasure = timeUnit;

                  const valueInOldUnits =
                    vote.roundDuration /
                    (1000 * 60 * { m: 1, h: 60, d: 1440 }[oldTimeMeasure]);

                  const newDurationInMs =
                    valueInOldUnits *
                    1000 *
                    60 *
                    { m: 1, h: 60, d: 1440 }[newTimeMeasure];

                  setTimeUnit(newTimeMeasure);
                  handleChange("roundDuration", newDurationInMs);
                }}
                label="Unité"
                data-round-duration-unit-select
              >
                <MenuItem value="m" data-round-duration-unit>
                  Minutes
                </MenuItem>
                <MenuItem value="h" data-round-duration-unit>
                  Heures
                </MenuItem>
                <MenuItem value="d" data-round-duration-unit>
                  Jours
                </MenuItem>
              </Select>
            </FormControl>
          </Box>
        </Box>
      </DialogContent>

      <DialogActions>
        {loading ? (
          <CircularProgress />
        ) : (
          <>
            <Button onClick={() => close()} variant="outlined">
              Annuler
            </Button>
            <Button variant="contained" type="submit" data-submit-create-vote>
              Créer
            </Button>
          </>
        )}
      </DialogActions>
    </Dialog>
  );
}

export default CreateVote;
