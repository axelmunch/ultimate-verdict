import { useEffect } from "react";
import "./App.css";
import { useApi } from "./ApiContext";

function App() {
  const { test } = useApi();

  useEffect(() => {
    test().then(console.log).catch(()=>{})
  }, []);

  return (
    <>
      <h1>Ultimate Verdict</h1>
    </>
  );
}

export default App;
