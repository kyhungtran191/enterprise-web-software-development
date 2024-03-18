import React from 'react'
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import AdminLayout from '@/layouts/AdminLayout.jsx'
import Home from './pages/general/Home'
import StudentContribution from './pages/client/StudentContribution'
import { RolesTable } from './components/RolesTable'
import { UsersTable } from './components/UsersTable'

function App() {
  return (
    <AdminLayout>
      <Routes>
        <Route path='/admin/roles' element={<RolesTable />} />
        <Route path='/admin/users' element={<UsersTable />} />
      </Routes>
    </AdminLayout>
  )
}

export default App
