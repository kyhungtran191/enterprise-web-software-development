import {
  createAcademicYearAPI,
  getAcademicYearByIdAPI,
  getAllAcademicYearsAPI,
  updateAcademicYearAPI
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
    await instanceAxios.put(`${updateAcademicYearAPI}`, data)
}
