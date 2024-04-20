import { getAccessTokenFromLS, getUserFromLS } from '@/utils/auth'
import React, { useEffect, useState } from 'react'
import { useQuery } from "@tanstack/react-query"
import { Auth } from '@/services/client'

const initialAppContext = {
  isAuthenticated: Boolean(getAccessTokenFromLS()),
  setIsAuthenticated: () => { },
  profile: getUserFromLS(),
  setProfile: () => { },
  roles: [],
  permission: [],
}

export const AppContext = React.createContext(initialAppContext)

export const AppProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(initialAppContext.isAuthenticated)
  const [profile, setProfile] = useState(initialAppContext.profile)
  const [avatar, setAvatar] = useState("")
  const [permission, setPermission] = useState([])
  const { isLoading, data } = useQuery({
    queryKey: ['profile'],
    queryFn: Auth.profile,
    enabled: !!isAuthenticated
  })
  let detailData = data && data?.data?.responseData
  useEffect(() => {
    setAvatar(detailData?.avatar)
  }, [detailData])

  return (
    <AppContext.Provider value={{ profile, setProfile, isAuthenticated, setIsAuthenticated, avatar, setAvatar, permission, setPermission }}>
      {children}
    </AppContext.Provider>
  )
}