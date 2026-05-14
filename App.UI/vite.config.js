import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/weatherforecast': {
        target: 'http://localhost:5021',
        changeOrigin: true,
      }
    }
  }
})
