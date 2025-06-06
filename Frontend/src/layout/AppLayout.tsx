import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import { Outlet, useNavigate } from "react-router-dom";

export default function AppLayout() {
  const navigate = useNavigate();

  return (
    <>
      <AppBar position="static">
        <Toolbar>
          <Typography
            variant="h3"
            onClick={() => navigate("/")}
            sx={{ cursor: "pointer" }}
          >
            Ultimate Verdict
          </Typography>
          <Button color="inherit" onClick={() => navigate("/home")}>
            Home
          </Button>
        </Toolbar>
      </AppBar>

      <>
        <Outlet />
      </>
    </>
  );
}
