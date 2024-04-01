import React, { SetStateAction, useState } from 'react'
import { User } from 'src/@types/auth'

import { getAccessTokenFromLS, getUserFromLS } from 'src/utils/auth'

const initialAppContext = {
  isAuthenticated: Boolean(getAccessTokenFromLS()),
  setIsAuthenticated: () => { },
  profile: getUserFromLS(),
  setProfile: () => { }
}

export const AppContext = React.createContext(initialAppContext)

export const AppProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(initialAppContext.isAuthenticated)
  const [profile, setProfile] = useState(initialAppContext.profile)
  const refetch = () => {
    setProfile(undefined)
    setIsAuthenticated(false)
  }
  return (
    <AppContext.Provider value={{ profile, setProfile, isAuthenticated, setIsAuthenticated }}>
      {children}
    </AppContext.Provider>
  )
}