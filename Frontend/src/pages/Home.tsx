import VoteListItem from "../components/VoteListItem";

function Home() {
  return (
    <>
      <ul>
        {[1, 2, 3, 4].map((item) => {
          return (
            <li key={item}>
              <VoteListItem id={item} />
            </li>
          );
        })}
      </ul>
    </>
  );
}

export default Home;
