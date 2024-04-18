import { useAppContext } from '@/hooks/useAppContext'
import React from 'react'
import { Navigate, Outlet } from 'react-router-dom'

export default function PermissionGuard({ permission }) {
  const { profile } = useAppContext()
  const permissions = profile?.permissions || []
  if (!permissions || permissions.length <= 0) return Navigate("/")
  return permissions.includes(permission) ? <Outlet></Outlet> : Navigate("/")
}
