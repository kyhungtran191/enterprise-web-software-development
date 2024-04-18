import Article from '@/components/article'
import { Skeleton } from '@/components/ui/skeleton'
import { Roles } from '@/constant/roles'
import { useAppContext } from '@/hooks/useAppContext'
import { Contributions } from '@/services/client'
import { useQuery } from '@tanstack/react-query'
import React from 'react'
import { Link } from 'react-router-dom'

export default function LatestContribution() {
  const { profile } = useAppContext()
  const { data, isLoading } = useQuery({
    queryKey: ['lastest-contributions'],
    queryFn: Contributions.getLatestContribution,
    enabled: profile.roles !== Roles.Guest
  })
  if (!data) return <></>
  return (
    <section className="my-4">
      <div className="flex items-center justify-between">
        <h2 className='my-2 text-2xl font-bold'>Latest Article</h2>
        <Link to="/contributions?sortBy=day" className='underline text-slate-600'>View More</Link>
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
        {data && data?.data?.responseData && data?.data?.responseData.length > 0 && data?.data?.responseData.map((article) => (<Article key={article.id} article={article}></Article>))}
      </div>
    </section>
  )
}
