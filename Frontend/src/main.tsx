import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { createTheme, ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';

const LIGHT_THEME_AVAILABLE = true;
const DARK_THEME_AVAILABLE = true;

const theme = createTheme({
  colorSchemes: {
    light: LIGHT_THEME_AVAILABLE && {
      palette: {},
    },
    dark: DARK_THEME_AVAILABLE && {
      palette: {},
    },
  },
});

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ThemeProvider theme={theme}>
      <CssBaseline enableColorScheme />
      <App />
    </ThemeProvider>
  </StrictMode>,
)
