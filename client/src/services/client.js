import { ContributionAPI, allAcademicYear, allFacultiesAPI, commentPublicAPI, editContributionsInfo, editProfileAPI, favoriteAPI, featuredContribution, forgotPasswordAPI, latestContribution, loginAPI, profileAPI, publicContributionAPI, readLaterListAPI, recentContributionAPI, resetPasswordAPI, toggleFavoriteAPI, toggleReadLaterAPI, topContributors, validateResetPasswordAPI } from "@/apis"
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
  getCurrentContribution: async (queryParams) =>
    await instanceAxios.get(recentContributionAPI, {
      params: queryParams
    }),
  addContribution: async (body) => {
    await instanceAxios.post(ContributionAPI, body)
  },
  getContributionEdit: async (slug) => await instanceAxios.get(`${editContributionsInfo}/${slug}`),
  updateContribution: async (body) => { await instanceAxios.put(ContributionAPI, body) },
  getFavoriteContribution: async (queryParams = {}) => await instanceAxios.get(favoriteAPI, {
    params: queryParams
  }),
  likeContribution: async (id) => await instanceAxios.post(`${toggleFavoriteAPI}/${id}`),
  getReadLater: async (queryParams = {}) => await instanceAxios.get(readLaterListAPI, {
    params: queryParams
  }),
  addReadLater: async (id) => await instanceAxios.post(`${toggleReadLaterAPI}/${id}`),
  commentPublic: async (data) => await instanceAxios.post(`${commentPublicAPI}/${data?.id}`, data?.body)
}

export const Auth = {
  login: async (body) => axios.post(loginAPI, body),
  forgotPassword: async (email) => axios.post(forgotPasswordAPI, {
    email
  }),
  validateToken: async (token) => axios.post(validateResetPasswordAPI, {
    token
  }),
  resetPassword: async (body) => axios.post(resetPasswordAPI, body),
  profile: async () => instanceAxios.get(profileAPI),
  updateProfile: async (body) => {
    await instanceAxios.post(editProfileAPI, body)
  },
}
