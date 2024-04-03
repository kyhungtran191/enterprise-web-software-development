import { allAcademicYear, allFacultiesAPI, featuredContribution, forgotPasswordAPI, latestContribution, loginAPI, publicContributionAPI, recentContributionAPI, resetPasswordAPI, topContributors, validateResetPasswordAPI } from "@/apis"
import instanceAxios from "@/utils/axiosInstance"
import axios from "axios"

export const Contributions = {
  getAllFaculties: async () => await axios.get(allFacultiesAPI),
  getTopContributors: async () => await axios.get(topContributors),
  getFeaturedContributions: async () => await axios.get(featuredContribution),
  getAllAcademicYear: async () => await axios.get(allAcademicYear),
  getLatestContribution: async () => await axios.get(latestContribution),
  getDetailPublicContribution: async (slug) => await axios.get(`${publicContributionAPI}/${slug}`),
  getAllPublicContribution: async (queryParams) => await axios.get(`${publicContributionAPI}/paging`, {
    params: queryParams
  }),
  getCurrentContribution: async (params) =>
    await instanceAxios.get(recentContributionAPI, {
      params: params
    })
}

export const Auth = {
  login: async (body) => axios.post(loginAPI, body),
  forgotPassword: async (email) => axios.post(forgotPasswordAPI, {
    email
  }),
  validateToken: async (token) => axios.post(validateResetPasswordAPI, {
    token
  }),
  resetPassword: async (body) => axios.post(resetPasswordAPI, body)
}
