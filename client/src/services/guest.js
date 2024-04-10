import { guestAPI } from "@/apis";
import instanceAxios from "@/utils/axiosInstance";

export const GuestContribution = {
  getContribution: (queryParams = {}) => instanceAxios.get(`${guestAPI}/`, {
    params: queryParams
  })
}