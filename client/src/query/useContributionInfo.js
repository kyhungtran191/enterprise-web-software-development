import { Contributions } from "@/services/client"
import { useQuery } from "@tanstack/react-query"
export const useContributionsEditDetail = (slug) => useQuery({
  queryKey: ['detailEditInfo'],
  queryFn: (_) => Contributions.getContributionEdit(slug)
})