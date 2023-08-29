import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      "/api": {
        target: "http://localhost:5253",
        changeOrigin: true,
        secure: false,
        headers: {
          Connection: "Keep-Alive",
        },
        rewrite: (path) => path.replace(/^\/api/, ""),
      },
    },
  },
});
