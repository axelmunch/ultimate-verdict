import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import dotenv from "dotenv";
import path from "path";

if (process.env.VITE_API_URL === undefined) {
  dotenv.config({ path: path.resolve(__dirname, "../.env") });
}

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    host: "0.0.0.0",
    port: 5173,
    watch: {
      usePolling: true,
      interval: 100,
    },
  },
});
