import List from "@mui/material/List";
import VoteListItem from "../components/VoteListItem";
import ListItem from "@mui/material/ListItem";

function Home() {
  return (
    <>
      <List>
        {[1, 2, 3, 4].map((item) => {
          return (
            <ListItem key={item}>
              <VoteListItem id={item} />
            </ListItem>
          );
        })}
      </List>
    </>
  );
}

export default Home;
