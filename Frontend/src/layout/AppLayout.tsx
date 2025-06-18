import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Link from "@mui/material/Link";
import Paper from "@mui/material/Paper";
import Slide from "@mui/material/Slide";
import { useTheme } from "@mui/material/styles";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import useScrollTrigger from "@mui/material/useScrollTrigger";
import { useEffect, useRef, useState } from "react";
import { Outlet, Link as RouterLink } from "react-router-dom";

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
          </Toolbar>
        </AppBar>
      </HideOnScroll>

      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          paddingTop: `${appBarHeight}px`,
          width: "100%",
          alignItems: "center",
          justifyContent: "center",
          minHeight: "100vh",
        }}
      >
        <Paper
          elevation={2}
          sx={{
            width: "100%",
            maxWidth: 1000,
            height: "100%",
            flex: 1,
            padding: 3,
          }}
        >
          <Outlet />
        </Paper>
      </Box>
    </>
  );
}
