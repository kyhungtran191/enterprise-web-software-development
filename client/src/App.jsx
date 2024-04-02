import { Route, Routes, useLocation } from 'react-router-dom'
import AdminLayout from './layouts/AdminLayout'
import { RolesTable } from './components/RolesTable'
import { UsersTable } from './components/UsersTable'
import GeneralLayout from './layouts'
import Home from './pages/general/Home'
import StudentContribution from './pages/client/manage/StudentContribution'
import Login from './pages/general/Login'
import Profile from './pages/client/manage/Profile'
import AddContribution from './pages/client/manage/contribution/AddContribution'
import { useEffect, useState } from 'react'
import Loading from './components/Loading'
import NotFound from './pages/404'
import { AcademicYearTable } from './components/AcademicYearTable'
import ContributionDetail from './pages/general/ContributionDetail'
import ForgotPassword from './pages/general/ForgotPassword'
import ResetPassword from './pages/general/ResetPassword'
import ContributionList from './pages/general/ContributionList'
import { ContributionTable } from './components/ContributionTable'
function App() {
  // const routes = useRoutesElements()
  const [loading, setLoading] = useState(true)
  const location = useLocation()
  useEffect(() => {
    setLoading(true) // Bắt đầu loading khi route thay đổi
    const timeoutId = setTimeout(() => {
      setLoading(false) // Dừng loading sau một khoảng thời gian
    }, 1000)
    window.scrollTo(0, 0)
    return () => clearTimeout(timeoutId) // Hủy bỏ timeout khi component unmount hoặc khi route thay đổi
  }, [location.pathname])

  return (
    <>
      {/* {loading && (
        <div className='fixed inset-0 z-50 flex flex-col items-center justify-center h-screen text-white bg-blue-800'>
          <img
            src='../greenwich-logo.png'
            alt='logo'
            className='md:w-[500px] h-auto object-cover flex-shrink-0 p-4'
          />
          <Loading></Loading>
        </div>
      )} */}
      { }
      <Routes>
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
          path='/'
          element={
            <GeneralLayout>
              <Home></Home>
            </GeneralLayout>
          }
        ></Route>
        <Route
          path='/manage/recent'
          element={<StudentContribution></StudentContribution>}
        ></Route>
        <Route path='/login' element={<Login></Login>}></Route>
        <Route path='/manage/profile' element={<Profile></Profile>}></Route>
        <Route
          path='/manage/add-contribution'
          element={<AddContribution></AddContribution>}
        ></Route>
        <Route
          path='/contributions'
          element={<ContributionList></ContributionList>}
        ></Route>
        <Route
          path='/contributions/:id'
          element={<ContributionDetail></ContributionDetail>}
        ></Route>
        <Route
          path='/forgot-password'
          element={<ForgotPassword></ForgotPassword>}
        ></Route>
        <Route
          path='/reset-password/:token'
          element={<ResetPassword></ResetPassword>}
        ></Route>
        <Route path='*' element={<NotFound></NotFound>}></Route>
      </Routes>
    </>
  )
}

export default App
