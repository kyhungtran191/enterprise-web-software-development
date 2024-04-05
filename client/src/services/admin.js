import {
  activateAcademicYearAPI,
  createAcademicYearAPI,
  createFacultyAPI,
  createRoleAPI,
  createUserAPI,
  deactivateAcademicYearAPI,
  deleteAcademicYearAPI,
  deleteFacultyAPI,
  deleteRoleAPI,
  getAcademicYearByIdAPI,
  getAllAcademicYearsAPI,
  getAllContributionsAPI,
  getAllFacultiesAPI,
  getAllRolesAPI,
  getAllUsersAPI,
  getFacultyByIdAPI,
  getPermissionsForRoleAPI,
  getRoleByIdAPI,
  getUserByIdAPI,
  updateAcademicYearAPI,
  updateFacultyAPI,
  updateRoleAPI,
  updateUserAPI
} from '@/apis'
import instanceAxios from '@/utils/axiosInstance'
export const AcademicYears = {
  getAllAcademicYears: async (queryParams) =>
    await instanceAxios.get(getAllAcademicYearsAPI, {
      params: queryParams
    }),
  createAcademicYear: async (data) =>
    await instanceAxios.post(createAcademicYearAPI, data),
  getAcademicYearById: async (id) =>
    await instanceAxios.get(`${getAcademicYearByIdAPI}/${id}`),
  updateAcademicYear: async (data) =>
    await instanceAxios.put(`${updateAcademicYearAPI}`, data),
  deleteAcademicYear: async (data) =>
    await instanceAxios.delete(deleteAcademicYearAPI, data),
  activateAcademicYear: async (id) =>
    await instanceAxios.post(`${activateAcademicYearAPI}/${id}`),
  deactivateAcademicYear: async (id) =>
    await instanceAxios.post(`${deactivateAcademicYearAPI}/${id}`)
}
export const Roles = {
  getAllRoles: async (queryParams) =>
    await instanceAxios.get(getAllRolesAPI, { params: queryParams }),
  getRoleById: async (id) => await instanceAxios.get(`${getRoleByIdAPI}/${id}`),
  createRole: async (data) => await instanceAxios.post(createRoleAPI, data),
  updateRoleById: async (id, data) =>
    await instanceAxios.put(`${updateRoleAPI}/${id}`, data),
  deleteRolesById: async (data) =>
    await instanceAxios.delete(deleteRoleAPI, data),
  getPermissionsForRole: async (id) =>
    instanceAxios.get(`${getPermissionsForRoleAPI}/${id}/permissions`),
  updateRolePermissions: async (data) =>
    await instanceAxios.put(getPermissionsForRoleAPI, data)
}
export const Users = {
  getAllUsers: async (queryParams) =>
    await instanceAxios.get(getAllUsersAPI, { params: queryParams }),
  getUserById: async (id) => await instanceAxios.get(`${getUserByIdAPI}/${id}`),
  createUser: async (data) =>
    await instanceAxios.post(createUserAPI, data, {
      headers: { 'Content-Type': 'multipart/form-data' }
    }),
  updateUserById: async (data) =>
    await instanceAxios.put(`${updateUserAPI}`, data)
}

export const Faculties = {
  getAllFaculties: async () =>
    await instanceAxios.get(`${getAllFacultiesAPI}/paging`),
  getFacultyById: async (id) =>
    await instanceAxios.get(`${getFacultyByIdAPI}/${id}`),
  createFaculty: async (data) =>
    await instanceAxios.post(createFacultyAPI, data),
  updateFaculty: async (data) =>
    await instanceAxios.put(updateFacultyAPI, data),
  deleteFaculty: async (id) =>
    await instanceAxios.delete(`${deleteFacultyAPI}/${id}`)
}

export const Contributions = {
  getAllContributions: async () =>
    await instanceAxios.get(`${getAllContributionsAPI}/paging`)
}
