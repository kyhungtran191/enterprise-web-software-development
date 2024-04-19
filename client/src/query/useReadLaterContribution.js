import { Contributions } from "@/services/client";
import { useQuery } from "@tanstack/react-query";
export function useReadLaterContribution(id) {
  const { data } = useQuery({
    queryKey: ['readlater-list'], queryFn: Contributions.getReadLater,
  })
  const detailData = data && data?.data?.responseData
  const currentList = detailData && detailData?.map((item) => item.id)
  return currentList?.includes(id)
}

