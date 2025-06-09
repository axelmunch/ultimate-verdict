import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Link from "@mui/material/Link";
import Slide from "@mui/material/Slide";
import { useTheme } from "@mui/material/styles";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import useScrollTrigger from "@mui/material/useScrollTrigger";
import { useEffect, useRef, useState } from "react";
import { Outlet, useNavigate, Link as RouterLink } from "react-router-dom";

interface HideOnScrollProps {
  children?: React.ReactElement;
}

function HideOnScroll({ children }: HideOnScrollProps) {
  const trigger = useScrollTrigger();

  return (
    <Slide appear={false} direction="down" in={!trigger}>
      {children as React.ReactElement}
    </Slide>
  );
}

export default function AppLayout() {
  const appBarRef = useRef<HTMLDivElement>(null);
  const [appBarHeight, setAppBarHeight] = useState(0);

  const navigate = useNavigate();
  const theme = useTheme();

  useEffect(() => {
    if (appBarRef.current) {
      setAppBarHeight(appBarRef.current.offsetHeight);
    }
  }, []);

  return (
    <>
      <HideOnScroll>
        <AppBar ref={appBarRef}>
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
      </HideOnScroll>

      <Box sx={{ paddingTop: `${appBarHeight}px` }}>
        <Outlet />
      </Box>
    </>
  );
}
