import { useAppContext } from '@/hooks/useAppContext'
import React from 'react'
import { Navigate, Outlet } from 'react-router-dom'

export default function RequireAuth({ children }) {
  const { isAuth } = useAppContext()
  if (!isAuth) return Navigate("/login")
  return (
    <>{children}
      <Outlet></Outlet>
    </>
  )
}
