import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemText from "@mui/material/ListItemText";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import { useTheme } from '@mui/material/styles';
import { useEffect, useState } from 'react';
import {
  DndContext,
  closestCenter,
  PointerSensor,
  useSensor,
  useSensors,
  TouchSensor,
  type DragEndEvent
} from '@dnd-kit/core';
import {
  arrayMove,
  SortableContext,
  useSortable,
  verticalListSortingStrategy,
} from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';

interface SortableItemProps {
  id: number;
  name: string;
  index: number;
}

interface RankingProps {
  options: string[];
  setCanSubmit: (canSubmit: boolean) => void;
}

interface Option {
  id: number;
  name: string;
};

function SortableItem({ id, name, index }: SortableItemProps) {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({ id });

  const theme = useTheme();

  const style = {
    ...(transform && { transform: CSS.Transform.toString(transform) }),
    ...(transition && { transition }),
    cursor: isDragging ? 'grabbing' : 'grab',
    position: 'relative',
    backgroundColor: isDragging ? theme.palette.background.paper : 'inherit',
    zIndex: isDragging ? 1 : 'auto',
  };

  return (
    <ListItem ref={setNodeRef} sx={style} {...attributes} {...listeners} divider>
      <ListItemText>
        {index + 1}
      </ListItemText>
      <ListItemText>
        <Typography variant="body1">
          {name}
        </Typography></ListItemText>
    </ListItem>
  );
}

function Ranking({ options: optionsProp, setCanSubmit }: RankingProps) {

  const [options, setOptions] = useState<Option[]>(optionsProp.map((name, index) => ({
    id: index,
    name
  })));

  const sensors = useSensors(useSensor(PointerSensor), useSensor(TouchSensor));

  useEffect(() => {
    setCanSubmit(true);
  }, [setCanSubmit]);

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;

    if (over && active.id !== over.id) {
      setOptions((items) => {
        const oldIndex = items.findIndex((item) => item.id === active.id);
        const newIndex = items.findIndex((item) => item.id === over.id);
        return arrayMove(items, oldIndex, newIndex);
      });
    }
  };

  return (
    <Paper>
      <Typography variant="h6" gutterBottom>
        Ranking
      </Typography>
      <Typography variant="body1">
        Instructions
      </Typography>
      <Button onClick={() => setOptions(optionsProp.map((name, index) => ({
        id: index,
        name
      })))}>Reset</Button>
      <DndContext
        sensors={sensors}
        collisionDetection={closestCenter}
        onDragEnd={handleDragEnd}
      >
        <SortableContext items={options.map((c) => c.id)} strategy={verticalListSortingStrategy}>
          <List sx={{ touchAction: 'none' }}>
            {options.map((candidate, index) => (
              <SortableItem
                key={candidate.id}
                id={candidate.id}
                name={candidate.name}
                index={index}
              />
            ))}
          </List>
        </SortableContext>
      </DndContext>
    </Paper>
  );
}

export default Ranking;
