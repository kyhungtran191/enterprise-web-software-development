import { Route, Routes } from "react-router-dom"
import AdminLayout from "./layouts/AdminLayout"
import useRoutesElements from "@/useRouteElements";
import { RolesTable } from "./components/RolesTable";
import { UsersTable } from "./components/UsersTable";
import GeneralLayout from "./layouts";
import Home from "./pages/general/Home";
import StudentContribution from "./pages/client/manage/StudentContribution";
import Login from "./pages/general/Login";
import Profile from "./pages/client/manage/Profile";


function App() {
  // const routes = useRoutesElements()
  return (

    <Routes>
      <Route path='/admin/roles' element={<AdminLayout>
        <RolesTable />
      </AdminLayout>} />
      <Route path='/admin/users' element={<AdminLayout><UsersTable /></AdminLayout>} />
      <Route path="/" element={<GeneralLayout><Home></Home></GeneralLayout>}>
      </Route>
      <Route path="/manage/recent" element={<StudentContribution></StudentContribution>}>
      </Route>
      <Route path="/login" element={<Login></Login>}>
      </Route>
      <Route path="/manage/profile" element={<Profile></Profile>}>
      </Route>
    </Routes>
  )
}

export default App
