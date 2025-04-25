import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
        '~bootstrap': path.resolve(__dirname, 'node_modules/bootstrap'),
    }
  },
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5166',
        changeOrigin: true,
        rewrite: (path) => {
          console.log('[Proxy Rewrite]', path);
          return path.replace(/^\/api/, '');
        }
      }
    },
    hot: true
  }
})