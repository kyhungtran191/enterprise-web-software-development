import Article from "@/components/article"
import { Skeleton } from "@/components/ui/skeleton"
import { Roles } from "@/constant/roles"
import { useAppContext } from "@/hooks/useAppContext"
import { useGuestContribution } from "@/query/useGuestContributions"
import { Link } from "react-router-dom"
export default function GuestContribution() {
  const { profile } = useAppContext()
  const { data, isLoading } = useGuestContribution()
  if (!data || profile?.roles !== Roles.Guest) return <></>
  console.log(data)
  const detailData = data && data?.data?.responseData?.results && data?.data?.responseData?.results
  return (
    <section className="my-4">
      <div className="flex items-center justify-between">
        <h2 className='my-2 text-2xl font-bold'>Guest Allowed Article</h2>
      </div>
      {isLoading && <div className="grid gap-3 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 xl:gap-4">
        {Array(4).fill(0).map((item, index) => (
          <div className="flex flex-col space-y-3" key={index}>
            <Skeleton className="h-[125px] w-[250px] rounded-xl" />
            <div className="space-y-2">
              <Skeleton className="h-4 w-[250px]" />
              <Skeleton className="h-4 w-[200px]" />
            </div>
          </div>
        ))}
      </div>}
      <div className="grid gap-3 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 xl:gap-4">
        {detailData?.map((article) => (<Article key={article.id} article={article}></Article>))}
      </div>
      {detailData && detailData.length <= 0 && <div className="text-xl font-bold text-center">No Data..</div>}
    </section>
  )
}
