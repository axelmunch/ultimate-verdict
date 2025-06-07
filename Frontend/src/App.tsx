import "./App.css";
import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import Home from "./pages/Home";
import AppLayout from "./layout/AppLayout";
import Vote from "./pages/Vote";
import Round from "./pages/Round";

function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route index element={<Navigate to="/home" />} />
          <Route path="/" element={<AppLayout />}>
            <Route path="home" element={<Home />} />
            <Route path="vote/:voteId" element={<Vote />}>
              <Route path="round/:roundId" element={<Round />} />
            </Route>
          </Route>
          <Route path="*" element={<Navigate to="/" />} />
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
