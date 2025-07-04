import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import ApiProvider from "./ApiContext";
import TimeProvider from "./TimeContext.tsx";

const LIGHT_THEME_AVAILABLE = true;
const DARK_THEME_AVAILABLE = true;

const theme = createTheme({
  colorSchemes: {
    light: LIGHT_THEME_AVAILABLE && {
      palette: {
        primary: {
          main: "#3f52e3",
        },
        secondary: {
          main: "#e33f40",
        },
      },
    },
    dark: DARK_THEME_AVAILABLE && {
      palette: {
        primary: {
          main: "#3f52e3",
        },
        secondary: {
          main: "#e33f40",
        },
      },
    },
  },
  components: {
    MuiAppBar: {
      styleOverrides: {
        root: ({ theme }) => ({
          backgroundColor: theme.palette.background.paper,
        }),
      },
    },
  },
});

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <ThemeProvider theme={theme}>
      <CssBaseline enableColorScheme />
      <TimeProvider>
        <ApiProvider>
          <App />
        </ApiProvider>
      </TimeProvider>
    </ThemeProvider>
  </StrictMode>,
);
