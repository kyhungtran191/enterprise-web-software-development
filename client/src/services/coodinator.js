import { MCContributionsAPI, MCContributionsApprove, MCContributionsReject } from "@/apis";
import instanceAxios from "@/utils/axiosInstance";

export const Contributions = {
  MCContribution: async (queryParams) =>
    await instanceAxios.get(MCContributionsAPI, {
      params: queryParams
    }),
  MCContributionDetail: async (slug) => await instanceAxios.get(`${MCContributionsAPI}/${slug}`),
  MCApprove: async (ids) => await instanceAxios.post(`${MCContributionsApprove}`, {
    ids
  }),
  MCReject: async (body) => await instanceAxios.post(`${MCContributionsReject}`, {
    body
  })
}