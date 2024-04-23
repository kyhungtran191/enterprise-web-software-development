import { Navigate, Outlet, Route, Routes, useLocation } from 'react-router-dom'
import AdminLayout from './layouts/AdminLayout'
import { RolesTable } from './components/RolesTable'
import { UsersTable } from './components/UsersTable'
import GeneralLayout from './layouts'
import Home from './pages/general/Home'
import StudentContribution from './pages/client/manage/StudentContribution'
import Login, { connection } from './pages/general/Login'
import Profile from './pages/client/manage/Profile'
import AddContribution from './pages/client/manage/contribution/AddContribution'
import { useContext, useEffect, useState } from 'react'
import Loading from './components/Loading'
import NotFound from './pages/404'
import { AcademicYearTable } from './components/AcademicYearTable'
import ContributionDetail from './pages/general/ContributionDetail'
import ForgotPassword from './pages/general/ForgotPassword'
import ResetPassword from './pages/general/ResetPassword'
import ContributionList from './pages/general/ContributionList'
import { ContributionTable } from './components/ContributionTable'
import UpdateContribution from './pages/client/manage/contribution/UpdateContribution'
import ManageContributions from './pages/coodinator/ManageContributions'
import { AppContext } from './contexts/app.context'
import PreviewContribution from './pages/general/PreviewContribution'
import SettingGAC from './pages/coodinator/SettingGAC'
import FavoriteContribution from './pages/client/manage/contribution/FavoriteContribution'
import { FacultiesTable } from './components/FacultiesTable'
import { Roles } from './constant/roles'
import ViewFile from './pages/general/ViewFile'
import Dashboard from './components/Dashboard'
import ReadLaterContribution from './pages/client/manage/contribution/ReadLaterContribution'
import StudentDashboard from './pages/client/manage/contribution/StudentDashboard'
import ChatPage from './pages/ChatPage'
import { routesConfig } from './constant/routeConfigs'
import { useAppContext } from './hooks/useAppContext'

import { getAccessTokenFromLS } from './utils/auth'
function App() {
  // const routes = useRoutesElements()
  const [loading, setLoading] = useState(true)
  const location = useLocation()
  // connection?.on("GetNewAnnouncement", (message) => console.log(message))
  useEffect(() => {
    setLoading(true)
    const timeoutId = setTimeout(() => {
      setLoading(false)
    }, 1000)
    window.scrollTo(0, 0)
    return () => clearTimeout(timeoutId)
  }, [location.pathname])

  const { isAuthenticated, profile, permission } = useContext(AppContext)

  const checkPermissions = (requiredPermission) => {
    if (!requiredPermission) return true
    if (!permission) return false

    const [category, type] = requiredPermission.split('.')
    const hasPermission =
      permission[category] &&
      permission[category][type] === `Permissions.${category}.${type}`
    return hasPermission
  }


  const renderRoute = (route, key) => {
    const {
      path,
      component: Component,
      layout: Layout,
      sidebarOptions,
      permission
    } = route
    const isAllowed = checkPermissions(permission)
    return (
      <Route
        key={key}
        path={path}
        element={
          isAllowed ? (
            Layout ? (
              <Layout links={sidebarOptions}>
                <Component />
              </Layout>
            ) : (
              <Component />
            )
          ) : isAuthenticated ? (
            <Navigate to='/404' replace />
          ) : (
            <Navigate to='/login' replace />
          )
        }
      />
    )
  }

  function RequireAuth() {
    return isAuthenticated ? <Outlet></Outlet> : <Navigate to='/login' />
  }

  function RejectedRoute() {
    return !isAuthenticated ? <Outlet></Outlet> : <Navigate to='/' />
  }
  function IsGuestAccount() {
    return profile.roles === Roles.Guest ? (
      <Navigate to='/' />
    ) : (
      <Outlet></Outlet>
    )
  }

  function IsStudent() {
    return profile.roles === Roles.Student ? (
      <Outlet></Outlet>
    ) : (
      <Navigate to='/' />
    )
  }
  function IsMC() {
    return profile.roles == Roles.Coordinator ? (
      <Outlet></Outlet>
    ) : (
      <Navigate to='/' />
    )
  }

  return (
    <>
      {loading && (
        <div className='fixed inset-0 z-50 flex flex-col items-center justify-center h-screen text-white bg-blue-800'>
          <img
            src='../../greenwich-logo.png'
            alt='logo'
            className='md:w-[500px] h-auto object-cover flex-shrink-0 p-4'
          />
          <Loading></Loading>
        </div>
      )}
      {/* <Routes>
        <Route path='' element={<RequireAuth></RequireAuth>}>
          <Route
            path='/'
            element={
              <GeneralLayout>
                <Home></Home>
              </GeneralLayout>
            }
          ></Route>
          {permission?.Roles?.View && (
            <Route
              path='/admin/roles'
              element={
                <AdminLayout>
                  <RolesTable />
                </AdminLayout>
              }
            />
          )}
          <Route
            path='/admin/users'
            element={
              <AdminLayout>
                <UsersTable />
              </AdminLayout>
            }
          />
          <Route
            path='/admin/academic-years'
            element={
              <AdminLayout>
                <AcademicYearTable />
              </AdminLayout>
            }
          />
          <Route
            path='/admin/contributions'
            element={
              <AdminLayout>
                <ContributionTable />
              </AdminLayout>
            }
          />
          <Route
            path='/admin/faculties'
            element={
              <AdminLayout>
                <FacultiesTable />
              </AdminLayout>
            }
          />
          <Route
            path='/admin/dashboard'
            element={
              <AdminLayout>
                <Dashboard />
              </AdminLayout>
            }
          />
          <Route element={<IsGuestAccount />} path=''>
            <Route path='/student-manage'>
              <Route
                index
                element={<StudentDashboard></StudentDashboard>}
              ></Route>
              <Route
                path='/student-manage/recent'
                element={<StudentContribution></StudentContribution>}
              ></Route>
              <Route
                path='/student-manage/add-contribution'
                element={<AddContribution></AddContribution>}
              ></Route>
              <Route
                path='/student-manage/edit-contribution/:slug'
                element={<UpdateContribution></UpdateContribution>}
              ></Route>
              <Route
                path='/student-manage/favorites'
                element={<FavoriteContribution></FavoriteContribution>}
              ></Route>
              <Route
                path='/student-manage/read-later'
                element={<ReadLaterContribution></ReadLaterContribution>}
              ></Route>
              <Route
                path='/student-manage/dashboard'
                element={<StudentDashboard></StudentDashboard>}
              ></Route>
            </Route>
          </Route>
          <Route path='/message' element={<ChatPage></ChatPage>}></Route>
          <Route
            path='/coodinator-manage/contributions'
            element={<ManageContributions></ManageContributions>}
          ></Route>
          <Route
            path='/coodinator-manage/setting-guest'
            element={<SettingGAC></SettingGAC>}
          ></Route>
          <Route element={<IsGuestAccount />}>
            <Route
              path='/contributions'
              element={<ContributionList></ContributionList>}
            ></Route>
            <Route
              path='/preview/:slug'
              element={<PreviewContribution></PreviewContribution>}
            ></Route>
            <Route path='/profile' element={<Profile></Profile>}></Route>
          </Route>
          <Route
            path='/contributions/:id'
            element={<ContributionDetail></ContributionDetail>}
          ></Route>
          <Route path='/view-file' element={<ViewFile></ViewFile>}></Route>
        </Route>
     
        <Route path='*' element={<NotFound></NotFound>}></Route>
      </Routes> */}
      <Routes>
        {routesConfig.map((route, index) => renderRoute(route, index))}
        <Route path='*' element={<NotFound />} />
        <Route path='' element={<RequireAuth></RequireAuth>}>
          {/* General */}
          <Route
            path='/'
            element={
              <GeneralLayout>
                <Home></Home>
              </GeneralLayout>
            }
          ></Route>
          <Route
            path='/contributions/:id'
            element={<ContributionDetail></ContributionDetail>}
          ></Route>
          <Route path='/view-file' element={<ViewFile></ViewFile>}></Route>

          <Route element={<IsGuestAccount />} path=''>
            {/* Student */}
            <Route path='/student-manage' element={<IsStudent></IsStudent>}>
              <Route
                index
                element={<StudentDashboard></StudentDashboard>}
              ></Route>
              <Route
                path='/student-manage/recent'
                element={<StudentContribution></StudentContribution>}
              ></Route>
              <Route
                path='/student-manage/add-contribution'
                element={<AddContribution></AddContribution>}
              ></Route>
              <Route
                path='/student-manage/edit-contribution/:slug'
                element={<UpdateContribution></UpdateContribution>}
              ></Route>
              <Route
                path='/student-manage/favorites'
                element={<FavoriteContribution></FavoriteContribution>}
              ></Route>
              <Route
                path='/student-manage/read-later'
                element={<ReadLaterContribution></ReadLaterContribution>}
              ></Route>
              <Route
                path='/student-manage/dashboard'
                element={<StudentDashboard></StudentDashboard>}
              ></Route>
            </Route>

            {/* Manager */}
            <Route path='/coodinator-manage' element={<IsMC></IsMC>}>
              <Route
                path='/coodinator-manage/contributions'
                element={<ManageContributions></ManageContributions>}
              ></Route>
              <Route
                path='/coodinator-manage/setting-guest'
                element={<SettingGAC></SettingGAC>}
              ></Route>
            </Route>

            {/* General */}

            <Route path='/message' element={<ChatPage></ChatPage>}></Route>
            <Route path='/profile' element={<Profile></Profile>}></Route>
            <Route
              path='/contributions'
              element={<ContributionList></ContributionList>}
            ></Route>
            <Route
              path='/preview/:slug'
              element={<PreviewContribution></PreviewContribution>}
            ></Route>
          </Route>
        </Route>
        <Route path='' element={<RejectedRoute></RejectedRoute>}>
          <Route path='/login' element={<Login></Login>} index></Route>
          <Route
            path='/forgot-password'
            element={<ForgotPassword></ForgotPassword>}
          ></Route>
          <Route
            path='/reset-password/:token'
            element={<ResetPassword></ResetPassword>}
          ></Route>
        </Route>
      </Routes>
    </>
  )
}

export default App
