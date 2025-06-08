import { useTheme } from "@mui/material";
import AppBar from "@mui/material/AppBar";
import Button from "@mui/material/Button";
import Link from "@mui/material/Link";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import { Outlet, useNavigate, Link as RouterLink } from "react-router-dom";

export default function AppLayout() {
  const navigate = useNavigate();
  const theme = useTheme();

  return (
    <>
      <AppBar position="static">
        <Toolbar>
          <Link component={RouterLink} to={"/"} underline="none">
            <Typography
              variant="h4"
              display="flex"
              alignItems="center"
              sx={{
                color: theme.palette.text.primary,
              }}
            >
              <img src="/ultimate_verdict.svg" style={{ height: "1em" }} />
              Ultimate Verdict
            </Typography>
          </Link>
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
