import { Navigate, Outlet, Route, Routes, useLocation } from 'react-router-dom'
import AdminLayout from './layouts/AdminLayout'
import { RolesTable } from './components/RolesTable'
import { UsersTable } from './components/UsersTable'
import GeneralLayout from './layouts'
import Home from './pages/general/Home'
import StudentContribution from './pages/client/manage/StudentContribution'
import Login from './pages/general/Login'
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
function App() {
  // const routes = useRoutesElements()
  const [loading, setLoading] = useState(true)
  const location = useLocation()

  useEffect(() => {
    setLoading(true)
    const timeoutId = setTimeout(() => {
      setLoading(false)
    }, 1000)
    window.scrollTo(0, 0)
    return () => clearTimeout(timeoutId)
  }, [location.pathname])

  function RequireAuth() {
    const { isAuthenticated } = useContext(AppContext)
    return isAuthenticated ? <Outlet></Outlet> : <Navigate to='/' />
  }

  function RejectedRoute() {
    const { isAuthenticated } = useContext(AppContext)
    return !isAuthenticated ? <Outlet></Outlet> : <Navigate to='/' />
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
      <Routes>
        <Route path="" element={<RequireAuth></RequireAuth>}>
          <Route
            path='/'
            element={
              <GeneralLayout>
                <Home></Home>
              </GeneralLayout>
            }
          ></Route>
          <Route
            path='/admin/roles'
            element={
              <AdminLayout>
                <RolesTable />
              </AdminLayout>
            }
          />
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
            path='/coodinator-manage/contributions'
            element={<ManageContributions></ManageContributions>}
          ></Route>
          <Route
            path='/contributions'
            element={<ContributionList></ContributionList>}
          ></Route>
          <Route
            path='/contributions/:id'
            element={<ContributionDetail></ContributionDetail>}
          ></Route>
          <Route path="/preview/:slug" element={<PreviewContribution></PreviewContribution>}></Route>
          <Route path='/profile' element={<Profile></Profile>}></Route>
        </Route>
        <Route path="" element={<RejectedRoute></RejectedRoute>}>
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
        <Route path='*' element={<NotFound></NotFound>}></Route>
      </Routes>
    </>
  )
}

export default App
