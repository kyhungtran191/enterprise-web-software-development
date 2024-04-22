import { AcademicYearTable } from '@/components/AcademicYearTable'
import { ContributionTable } from '@/components/ContributionTable'
import Dashboard from '@/components/Dashboard'
import { FacultiesTable } from '@/components/FacultiesTable'
import { RolesTable } from '@/components/RolesTable'
import { UsersTable } from '@/components/UsersTable'
import GeneralLayout from '@/layouts'
import AdminLayout from '@/layouts/AdminLayout'
import Home from '@/pages/general/Home'
import { ADMIN_OPTIONS, MM_OPTIONS } from './menuSidebar'

export const routesConfig = [
  // { path: '/', component: Home, layout: GeneralLayout, permission: null },
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
    sidebarOptions: ADMIN_OPTIONS,
    permission: 'Roles.View'
  },
  {
    path: '/admin/users',
    component: UsersTable,
    layout: AdminLayout,
    sidebarOptions: ADMIN_OPTIONS,
    permission: 'Users.View'
  },
  {
    path: '/admin/academic-years',
    component: AcademicYearTable,
    layout: AdminLayout,
    sidebarOptions: ADMIN_OPTIONS,
    permission: 'AcademicYears.View'
  },
  {
    path: '/admin/contributions',
    component: ContributionTable,
    layout: AdminLayout,
    sidebarOptions: ADMIN_OPTIONS,
    permission: 'Contributions.View'
  },
  {
    path: '/admin/faculties',
    component: FacultiesTable,
    layout: AdminLayout,
    sidebarOptions: ADMIN_OPTIONS,
    permission: 'Faculties.View'
  },
  {
    path: '/admin/dashboard',
    component: Dashboard,
    layout: AdminLayout,
    sidebarOptions: ADMIN_OPTIONS,
    permission: 'Dashboard.View'
  },

  {
    path: '/mm/dashboard',
    component: Dashboard,
    layout: AdminLayout,
    sidebarOptions: MM_OPTIONS,
    permission: 'Dashboard.View'
  },
  {
    path: '/mm/users',
    component: UsersTable,
    layout: AdminLayout,
    sidebarOptions: MM_OPTIONS,
    permission: 'Users.View'
  },
  {
    path: '/mm/contributions',
    component: ContributionTable,
    layout: AdminLayout,
    sidebarOptions: MM_OPTIONS,
    permission: 'Contributions.View'
  }

  // { path: '*', component: NotFound, layout: null, permission: null }
]
