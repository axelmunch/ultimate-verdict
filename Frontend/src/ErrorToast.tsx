import Alert from "@mui/material/Alert";
import Snackbar from "@mui/material/Snackbar";

interface ErrorToastProps {
  open: boolean;
  message: string;
  onClose: () => void;
}

function ErrorToast({ open, message, onClose }: ErrorToastProps) {
  return (
    <Snackbar
      open={open}
      autoHideDuration={5000}
      onClose={onClose}
      anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
    >
      <Alert onClose={onClose} severity="error">
        {message}
      </Alert>
    </Snackbar>
  );
}

export default ErrorToast;
