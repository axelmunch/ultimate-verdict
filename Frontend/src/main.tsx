import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import ApiProvider from "./ApiContext";

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
});

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <ThemeProvider theme={theme}>
      <CssBaseline enableColorScheme />
      <ApiProvider>
        <App />
      </ApiProvider>
    </ThemeProvider>
  </StrictMode>,
);
