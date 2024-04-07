import { Contributions } from "@/services/coodinator"
import { useQuery } from "@tanstack/react-query"
export const usePreviewContribution = (slug) => useQuery({
  queryKey: ['preview'],
  queryFn: (_) => Contributions.MCPreview(slug)
})