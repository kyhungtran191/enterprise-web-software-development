import Contributor from '@/components/contributor'
import { Skeleton } from '@/components/ui/skeleton'
import { Contributions } from '@/services/client'
import { useQuery } from '@tanstack/react-query'
import React from 'react'

export default function TopContributors() {
  const { data, isLoading } = useQuery({ queryKey: ['contributors'], queryFn: Contributions.getTopContributors })
  return (
    <section className="hidden my-4 sm:block">
      <h2 className='text-2xl font-bold'>Top Contributor</h2>
      <div className="grid grid-cols-2 gap-10 my-10 medium:grid-cols-3">
        {isLoading && Array(6).fill(0).map((item, index) => (<div className="flex items-center space-x-4" key={index}>
          <Skeleton className="w-12 h-12 rounded-full" />
          <div className="space-y-2">
            <Skeleton className="h-4 w-[250px]" />
            <Skeleton className="h-4 w-[200px]" />
          </div>
        </div>))}

        {data && data?.data?.responseData && data?.data?.responseData.map((item) => (
          <Contributor info={item} key={item.avatar}></Contributor>
        ))}
      </div>
    </section>
  )
}
