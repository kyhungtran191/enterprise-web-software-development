import { MCAddGuest, MCAllowGuest, MCComment, MCContributionsAPI, MCContributionsApprove, MCContributionsReject, MCPreviewContribution } from "@/apis";
import instanceAxios from "@/utils/axiosInstance";

export const Contributions = {
  MCContribution: async (queryParams) => await instanceAxios.get(MCContributionsAPI, {
    params: queryParams
  }),
  MCContributionDetail: async (slug) => await instanceAxios.get(`${MCContributionsAPI}/${slug}`),
  MCApprove: async (body) => await instanceAxios.post(`${MCContributionsApprove}`, body),
  MCReject: async (body) => await instanceAxios.post(`${MCContributionsReject}`, body),
  MCPreview: async (slug) => await instanceAxios.get(`${MCPreviewContribution}/${slug}`),
  MCComment: async (data) => await instanceAxios.post(`${MCComment}/${data?.id}`, data?.body),
  MCAllowGuest: async (data) => await instanceAxios.post(MCAllowGuest, data),
  MCCreateGuest: async (body) => await instanceAxios.post(MCAddGuest, body)
}