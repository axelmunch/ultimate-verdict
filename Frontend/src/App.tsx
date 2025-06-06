import { useEffect } from "react";
import "./App.css";
import { useApi } from "./ApiContext";
import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import Home from "./pages/Home";
import AppLayout from "./layout/AppLayout";

function App() {
  const { test } = useApi();

  useEffect(() => {
    test()
      .then(console.log)
      .catch(() => {});
  }, []);

  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Navigate to="/home" />} />
          <Route path="/" element={<AppLayout />}>
            <Route path="/home" element={<Home />} />
          </Route>
          <Route path="*" element={<Navigate to="/" />} />
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
