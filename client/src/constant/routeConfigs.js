import { AcademicYearTable } from '@/components/AcademicYearTable'
import { ContributionTable } from '@/components/ContributionTable'
import Dashboard from '@/components/Dashboard'
import { FacultiesTable } from '@/components/FacultiesTable'
import { RolesTable } from '@/components/RolesTable'
import { UsersTable } from '@/components/UsersTable'
import GeneralLayout from '@/layouts'
import AdminLayout from '@/layouts/AdminLayout'
import NotFound from '@/pages/404'
import ChatPage from '@/pages/ChatPage'
import AddContribution from '@/pages/client/manage/contribution/AddContribution'
import FavoriteContribution from '@/pages/client/manage/contribution/FavoriteContribution'
import ReadLaterContribution from '@/pages/client/manage/contribution/ReadLaterContribution'
import StudentDashboard from '@/pages/client/manage/contribution/StudentDashboard'
import UpdateContribution from '@/pages/client/manage/contribution/UpdateContribution'
import Profile from '@/pages/client/manage/Profile'
import StudentContribution from '@/pages/client/manage/StudentContribution'
import ManageContribution from '@/pages/coodinator/ManageContributions'
import SettingGAC from '@/pages/coodinator/SettingGAC'
import ContributionDetail from '@/pages/general/ContributionDetail'
import ContributionList from '@/pages/general/ContributionList'
import ForgotPassword from '@/pages/general/ForgotPassword'
import Home from '@/pages/general/Home'
import Login from '@/pages/general/Login'
import PreviewContribution from '@/pages/general/PreviewContribution'
import ResetPassword from '@/pages/general/ResetPassword'
import ViewFile from '@/pages/general/ViewFile'

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
  {
    path: '/student-manage',
    component: StudentDashboard,
    layout: null,
    permission: 'StudentDashboard.View'
  },
  {
    path: '/student-manage/recent',
    component: StudentContribution,
    layout: GeneralLayout,
    permission: 'StudentContribution.View'
  },
  {
    path: '/student-manage/add-contribution',
    component: AddContribution,
    layout: null,
    permission: 'AddContribution.View'
  },
  {
    path: '/student-manage/edit-contribution/:slug',
    component: UpdateContribution,
    layout: null,
    permission: 'EditContribution.View'
  },
  {
    path: '/student-manage/favorites',
    component: FavoriteContribution,
    layout: null,
    permission: 'FavoriteContribution.View'
  },
  {
    path: '/student-manage/read-later',
    component: ReadLaterContribution,
    layout: null,
    permission: 'ReadLaterContribution.View'
  },
  {
    path: '/message',
    component: ChatPage,
    layout: GeneralLayout,
    permission: null
  },
  {
    path: '/coodinator-manage/contributions',
    component: ManageContribution,
    layout: GeneralLayout,
    permission: 'ManageContributions.View'
  },
  {
    path: '/coodinator-manage/setting-guest',
    component: SettingGAC,
    layout: GeneralLayout,
    permission: 'SettingGAC.View'
  },
  {
    path: '/contributions',
    component: ContributionList,
    layout: null,
    permission: null
  },
  {
    path: '/preview/:slug',
    component: PreviewContribution,
    layout: GeneralLayout,
    permission: null
  },
  {
    path: '/profile',
    component: Profile,
    layout: GeneralLayout,
    permission: null
  },
  {
    path: '/contributions/:id',
    component: ContributionDetail,
    layout: null,
    permission: null
  },
  {
    path: '/view-file',
    component: ViewFile,
    layout: GeneralLayout,
    permission: null
  }
  // { path: '*', component: NotFound, layout: null, permission: null }
]
