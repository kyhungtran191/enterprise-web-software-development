import { MCContributionsAPI, MCContributionsApprove, MCContributionsReject } from "@/apis";
import instanceAxios from "@/utils/axiosInstance";

export const Contributions = {
  MCContribution: async (queryParams) => await instanceAxios.get(MCContributionsAPI, {
    params: queryParams
  }),
  MCContributionDetail: async (slug) => await instanceAxios.get(`${MCContributionsAPI}/${slug}`),
  MCApprove: async (body) => await instanceAxios.post(`${MCContributionsApprove}`, body),
  MCReject: async (body) => await instanceAxios.post(`${MCContributionsReject}`, {
    body
  })
}