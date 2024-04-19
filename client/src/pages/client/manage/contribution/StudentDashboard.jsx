import Dashboard from '@/components/Dashboard'
import { STUDENT_OPTIONS } from '@/constant/menuSidebar'
import AdminLayout from '@/layouts/AdminLayout'
import React from 'react'

export default function StudentDashboard() {
  return (
    <AdminLayout links={STUDENT_OPTIONS}>
      <Dashboard></Dashboard>
    </AdminLayout>
  )
}
