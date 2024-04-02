import { allFacultiesAPI, featuredContribution, latestContribution, publicContributionAPI, topContributors } from "@/apis"
import axios from "axios"

export const Contributions = {
  getAllFaculties: async () => await axios.get(allFacultiesAPI),
  getTopContributors: async () => await axios.get(topContributors),
  getFeaturedContributions: async () => await axios.get(featuredContribution),
  getLatestContribution: async () => await axios.get(latestContribution),
  getDetailPublicContribution: async (slug) => await axios.get(`${publicContributionAPI}/${slug}`)

}