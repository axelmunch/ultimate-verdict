import List from "@mui/material/List";
import VoteListItem from "../components/VoteListItem";
import ListItem from "@mui/material/ListItem";
import { useApi } from "../ApiContext";
import { useEffect, useState } from "react";
import { type VoteInput, type Vote } from "../types";
import Fab from "@mui/material/Fab";
import AddIcon from "@mui/icons-material/Add";
import CloseIcon from "@mui/icons-material/Close";
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
import dayjs from "dayjs";
import { DemoContainer } from "@mui/x-date-pickers/internals/demo";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DateTimePicker } from "@mui/x-date-pickers/DateTimePicker";
import Box from "@mui/material/Box";
import DeleteIcon from "@mui/icons-material/Delete";
import Alert from "@mui/material/Alert";

function Home() {
  const [votes, setVotes] = useState<Vote[]>([]);

  const [createVote, setCreateVote] = useState<VoteInput>({
    name: "",
    description: "",
    visibility: "public",
    type: "plural",
    nbRounds: 1,
    winnersByRound: [1],
    victoryCondition: "majority",
    replayOnDraw: false,
    options: [""],
    startDate: dayjs().unix() * 1000,
    roundDuration: 1000 * 60 * 60,
  });
  const [createVoteDialogOpen, setCreateVoteDialogOpen] = useState(false);

  const [timeUnit, setTimeUnit] = useState<"m" | "h" | "d">("h");

  const { getVotes } = useApi();

  useEffect(() => {
    getVotes().then(setVotes).catch(console.error);
  }, [getVotes]);

  const handleChange = (field: string, value: unknown) => {
    setCreateVote({ ...createVote, [field]: value });
  };

  const handleWinnersByRoundChange = (index: number, value: number) => {
    if (isNaN(value) || value < 1) return;

    if (index > 0 && value > createVote.winnersByRound[index - 1]) {
      return;
    }

    if (value > createVote.options.length) {
      value = createVote.options.length;
    }

    const newWinners = [...createVote.winnersByRound];
    newWinners[index] = value;

    for (let i = index + 1; i < newWinners.length; i++) {
      if (newWinners[i] > value) {
        newWinners[i] = value;
      }
    }

    setCreateVote({ ...createVote, winnersByRound: newWinners });
  };

  return (
    <>
      <List>
        {votes.map((vote) => {
          return (
            <ListItem key={vote.id}>
              <VoteListItem vote={vote} />
            </ListItem>
          );
        })}
      </List>
      <Fab
        onClick={() => setCreateVoteDialogOpen(true)}
        variant="extended"
        color="primary"
      >
        <AddIcon sx={{ mr: 1 }} />
        Vote
      </Fab>

      <Dialog
        open={createVoteDialogOpen}
        fullWidth
        slotProps={{
          paper: {
            component: "form",
            onSubmit: (event: React.FormEvent<HTMLFormElement>) => {
              event.preventDefault();

              console.log(createVote);
              setCreateVoteDialogOpen(false);
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
          <IconButton onClick={() => setCreateVoteDialogOpen(false)}>
            <CloseIcon />
          </IconButton>
        </DialogTitle>
        <Divider />

        <DialogContent>
          <TextField
            label="Nom"
            value={createVote.name}
            onChange={(event) => handleChange("name", event.target.value)}
            fullWidth
            required
            margin="normal"
          />
          <TextField
            label="Description"
            value={createVote.description}
            onChange={(event) =>
              handleChange("description", event.target.value)
            }
            fullWidth
            multiline
            rows={2}
            margin="normal"
          />

          <Divider sx={{ marginY: 2 }} />

          <FormControl fullWidth margin="normal">
            <InputLabel>Visibilité</InputLabel>
            <Select
              value={createVote.visibility}
              onChange={(event) =>
                handleChange("visibility", event.target.value)
              }
              label="Visibilité"
            >
              <MenuItem value="public">Publique</MenuItem>
              <MenuItem value="private">Privée</MenuItem>
            </Select>
          </FormControl>

          <FormControl fullWidth margin="normal">
            <InputLabel>Type</InputLabel>
            <Select
              value={createVote.type}
              onChange={(event) => {
                if (event.target.value === "elo") {
                  setCreateVote((prev) => ({
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
            >
              <MenuItem value="plural">Plural</MenuItem>
              <MenuItem value="ranked">Classement</MenuItem>
              <MenuItem value="weighted">Pondéré</MenuItem>
              <MenuItem value="elo">ELO</MenuItem>
            </Select>
          </FormControl>

          <FormControl fullWidth margin="normal">
            <InputLabel>Condition de victoire</InputLabel>
            <Select
              value={createVote.victoryCondition}
              onChange={(event) => {
                if (event.target.value === "last man standing") {
                  setCreateVote((prev) => ({
                    ...prev,
                    victoryCondition: event.target.value,
                    nbRounds: 1,
                    winnersByRound: [1],
                  }));

                  return;
                } else if (event.target.value === "none") {
                  setCreateVote((prev) => ({
                    ...prev,
                    victoryCondition: event.target.value,
                    replayOnDraw: false,
                  }));

                  return;
                }

                handleChange("victoryCondition", event.target.value);
              }}
              label="Condition de victoire"
              disabled={createVote.type === "elo"}
            >
              <MenuItem value="none">Aucune</MenuItem>
              <MenuItem value="majority">Majorité</MenuItem>
              <MenuItem value="absolute majority">Majorité absolue</MenuItem>
              <MenuItem value="2/3 majority">Majorité 2/3</MenuItem>
              <MenuItem value="last man standing">Elimination</MenuItem>
            </Select>
          </FormControl>

          <FormControlLabel
            control={
              <Switch
                checked={createVote.replayOnDraw}
                onChange={(event) =>
                  handleChange("replayOnDraw", event.target.checked)
                }
                disabled={
                  createVote.type === "elo" ||
                  createVote.victoryCondition === "none"
                }
              />
            }
            label="Rejeu en cas d'égalité"
          />

          <Divider sx={{ marginY: 2 }} />

          {createVote.type === "elo" && createVote.options.length > 4 ? (
            <Alert severity="warning">
              La création d'un vote ELO avec {createVote.options.length} options
              entrainera la création de{" "}
              {(createVote.options.length * (createVote.options.length - 1)) /
                2}{" "}
              tours
            </Alert>
          ) : null}

          <List>
            {createVote.options.map((option, index) => (
              <ListItem key={index} sx={{ paddingX: 0 }}>
                <TextField
                  label={`Option ${index + 1}`}
                  value={option}
                  onChange={(event) => {
                    const newOptions = [...createVote.options];
                    newOptions[index] = event.target.value;
                    handleChange("options", newOptions);
                  }}
                  fullWidth
                  required
                />
                <IconButton
                  onClick={() => {
                    const newOptions = [...createVote.options];
                    newOptions.splice(index, 1);

                    const newWinnersByRound = createVote.winnersByRound.map(
                      (val) =>
                        val > newOptions.length ? newOptions.length : val,
                    );

                    setCreateVote((prev) => ({
                      ...prev,
                      winnersByRound: newWinnersByRound,
                      options: newOptions,
                    }));
                  }}
                  disabled={createVote.options.length <= 1}
                >
                  <DeleteIcon />
                </IconButton>
              </ListItem>
            ))}
          </List>
          <Button
            onClick={() => {
              handleChange("options", [...createVote.options, ""]);
            }}
            startIcon={<AddIcon />}
            variant="outlined"
          >
            Option
          </Button>

          <Divider sx={{ marginY: 2 }} />

          <TextField
            type="number"
            label="Nombre de tours"
            value={createVote.nbRounds}
            onChange={(event) => {
              const value = Math.max(
                1,
                Math.min(5, parseInt(event.target.value) || 1),
              );

              const newWinnersByRound = createVote.winnersByRound.slice(
                0,
                value,
              );
              while (newWinnersByRound.length < value) {
                newWinnersByRound.push(1);
              }
              setCreateVote({
                ...createVote,
                nbRounds: value,
                winnersByRound: newWinnersByRound,
              });
            }}
            disabled={
              createVote.type === "elo" ||
              createVote.victoryCondition === "last man standing"
            }
            fullWidth
            margin="normal"
          />

          <Box display="flex" flexWrap="wrap" gap={2} marginTop={2}>
            {createVote.winnersByRound.map((val, index) => (
              <TextField
                key={index}
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
                  createVote.type === "elo" ||
                  createVote.victoryCondition === "last man standing"
                }
                sx={{ flexGrow: 1 }}
              />
            ))}
          </Box>

          <Divider sx={{ marginY: 2 }} />

          <LocalizationProvider dateAdapter={AdapterDayjs}>
            <DemoContainer components={["DateTimePicker", "DateTimePicker"]}>
              <DateTimePicker
                label="Date de début"
                value={dayjs(createVote.startDate)}
                onChange={(value) => {
                  if (value) {
                    handleChange("startDate", value.unix() * 1000);
                  }
                }}
              />
            </DemoContainer>
          </LocalizationProvider>

          <Box display="flex" gap={2}>
            <Box width="100%">
              <TextField
                type="number"
                label={`Durée des tours (${{ m: "minutes", h: "heures", d: "jours" }[timeUnit]})`}
                value={
                  createVote.roundDuration /
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
                      createVote.roundDuration /
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
                >
                  <MenuItem value="m">Minutes</MenuItem>
                  <MenuItem value="h">Heures</MenuItem>
                  <MenuItem value="d">Jours</MenuItem>
                </Select>
              </FormControl>
            </Box>
          </Box>
        </DialogContent>

        <DialogActions>
          <Button
            onClick={() => setCreateVoteDialogOpen(false)}
            variant="outlined"
          >
            Annuler
          </Button>
          <Button variant="contained" type="submit">
            Créer
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}

export default Home;
