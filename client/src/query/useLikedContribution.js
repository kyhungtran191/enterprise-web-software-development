import { Contributions } from "@/services/client";
import { useQuery } from "@tanstack/react-query";
export function useLikedContribution(id) {
  const { data } = useQuery({
    queryKey: ['favorite-list'], queryFn: Contributions.getFavoriteContribution,
  })
  const detailData = data && data?.data?.responseData
  const currentList = detailData && detailData?.map((item) => item.id)
  return currentList?.includes(id)
}

