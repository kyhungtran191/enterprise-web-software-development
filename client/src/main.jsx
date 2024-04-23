import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './index.css'
import { BrowserRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { AppProvider } from './contexts/app.context.jsx'
import { AxiosInterceptor } from './utils/axiosInstance.jsx'
import { ToastContainer } from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import { AbilityContext } from './components/casl/Can.js'
import Ability from './components/casl/Ability.js'
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      staleTime: 60000,
      cacheTime: 600000,
    }
  }
})


ReactDOM.createRoot(document.getElementById('root')).render(
  <AbilityContext.Provider value={Ability}>
    <BrowserRouter>
      <ToastContainer
        closeOnClick
        pauseOnHover={false}
        pauseOnFocusLoss={false}
      />
      <QueryClientProvider client={queryClient}>
        <AppProvider>
          <AxiosInterceptor>
            <App />
          </AxiosInterceptor>
        </AppProvider>
        <ReactQueryDevtools initialIsOpen={false} />
      </QueryClientProvider>
    </BrowserRouter>
  </AbilityContext.Provider>
)
