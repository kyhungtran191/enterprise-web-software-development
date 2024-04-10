import Article from '@/components/article'
import { Skeleton } from '@/components/ui/skeleton'
import { Roles } from '@/constant/roles'
import { useAppContext } from '@/hooks/useAppContext'
import { Contributions } from '@/services/client'
import { useQuery } from '@tanstack/react-query'
import React from 'react'

export default function FeaturedContribution() {
  const { profile } = useAppContext()
  const { data, isLoading } = useQuery({ queryKey: ['featured-contributions'], queryFn: Contributions.getFeaturedContributions, enabled: profile.roles !== Roles.Guest })
  if (!data) return <></>
  return (
    <section className="my-4">
      <h2 className='my-2 text-2xl font-bold'>Featured Article</h2>
      <div className="grid grid-cols-12 grid-rows-3 gap-4">
        {isLoading && <>
          <div className="col-span-12 row-span-3 medium:col-span-6">
            <div className="flex flex-col space-y-3">
              <Skeleton className="md:h-[360px] h-[400px] medium:h-[600px] w-[full] rounded-xl" />
              <div className="space-y-2">
                <Skeleton className="w-full h-10" />
                <Skeleton className="w-full h-10" />
              </div>
            </div>
          </div>
          <div className="col-span-12 row-span-1 medium:col-span-6">
            <div className="flex flex-col space-y-3">
              <Skeleton className="h-[120px] w-[full] rounded-xl" />
              <div className="space-y-2">
                <Skeleton className="w-full h-10" />
                <Skeleton className="w-full h-10" />
              </div>
            </div>
            <div className="flex flex-col space-y-3">
              <Skeleton className="h-[120px] w-[full] rounded-xl" />
              <div className="space-y-2">
                <Skeleton className="w-full h-10" />
                <Skeleton className="w-full h-10" />
              </div>
            </div>
            <div className="flex flex-col space-y-3">
              <Skeleton className="h-[120px] w-[full] rounded-xl" />
              <div className="space-y-2">
                <Skeleton className="w-full h-10" />
                <Skeleton className="w-full h-10" />
              </div>
            </div>
          </div>

        </>}
        {data && data?.data?.responseData && data?.data?.responseData.length > 0 &&
          <>
            <div className="col-span-12 row-span-3 medium:col-span-6">
              <Article article={data?.data?.responseData[0]} classImageCustom={"md:h-[600px]"}></Article>
            </div>
            {data?.data?.responseData.slice(1).map((item, index) => (
              <div className="col-span-12 row-span-1 medium:col-span-6" key={item.id}>
                <Article isRevert={true} classImageCustom={"md:h-[200px] md:w-[56%]"} article={item}></Article>
              </div>
            ))}
          </>}
      </div>
    </section>
  )
}
