import { AcademicYearTable } from '@/components/AcademicYearTable'
import { ContributionTable } from '@/components/ContributionTable'
import Dashboard from '@/components/Dashboard'
import { FacultiesTable } from '@/components/FacultiesTable'
import { RolesTable } from '@/components/RolesTable'
import { UsersTable } from '@/components/UsersTable'
import GeneralLayout from '@/layouts'
import AdminLayout from '@/layouts/AdminLayout'
import Home from '@/pages/general/Home'


export const routesConfig = [
  { path: '/', component: Home, layout: GeneralLayout, permission: null },
  // { path: '/login', component: Login, layout: GeneralLayout, permission: null },
  // {
  //   path: '/forgot-password',
  //   component: ForgotPassword,
  //   layout: GeneralLayout,
  //   permission: null
  // },
  // {
  //   path: '/reset-password/:token',
  //   component: ResetPassword,
  //   layout: GeneralLayout,
  //   permission: null
  // },
  {
    path: '/admin/roles',
    component: RolesTable,
    layout: AdminLayout,
    permission: 'Roles.View'
  },
  {
    path: '/admin/users',
    component: UsersTable,
    layout: AdminLayout,
    permission: 'Users.View'
  },
  {
    path: '/admin/academic-years',
    component: AcademicYearTable,
    layout: AdminLayout,
    permission: 'AcademicYears.View'
  },
  {
    path: '/admin/contributions',
    component: ContributionTable,
    layout: AdminLayout,
    permission: 'Contributions.View'
  },
  {
    path: '/admin/faculties',
    component: FacultiesTable,
    layout: AdminLayout,
    permission: 'Faculties.View'
  },
  {
    path: '/admin/dashboard',
    component: Dashboard,
    layout: AdminLayout,
    permission: 'Dashboard.View'
  },

  // { path: '*', component: NotFound, layout: null, permission: null }
]
