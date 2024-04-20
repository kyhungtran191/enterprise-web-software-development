import { Carousel, CarouselContent, CarouselItem, CarouselNext, CarouselPrevious } from '@/components/ui/carousel'
import { Skeleton } from '@/components/ui/skeleton'
import { useFaculty } from '@/query/useFaculty'
import { Contributions } from '@/services/client'
import { useQueries, useQuery } from '@tanstack/react-query'
import React from 'react'
import { Link } from 'react-router-dom'

export default function Faculty() {
  const { data, isLoading } = useFaculty()
  if (!data) return <></>
  return (
    <section className="my-4">
      <h2 className='text-2xl font-bold'>Faculty</h2>
      {isLoading && <div className="grid grid-cols-4 h-[160px] gap-4 my-4 md:h-auto md:grid-cols-3 medium:grid-cols-4">
        {Array(4).fill(0).map((item, index) => (<> <div className="flex flex-col space-y-3">
          <Skeleton className="h-[250px] rounded-xl" />
        </div></>))}
      </div>
      }
      <Carousel>
        <CarouselContent className="py-4">
          {data && data?.data?.responseData?.results.map((item, index) => (
            <CarouselItem className="basis-1/2 md:basis-1/3 medium:basis-1/4" key={item.id}>
              <Link to={`/contributions?facultyname=${item.name}`} className="h-[180px] md:h-[234px] flex flex-col items-center justify-center gap-3 transition duration-300 ease-in-out rounded-md shadow-lg hover:-translate-y-2">
                <img src="./falcuty-icon.png" alt="" className='w-[32px] h-[32px] flex-shrink-0' />
                <h3 className='font-semibold'>{item.name}</h3>
              </Link>
            </CarouselItem>
          ))}
        </CarouselContent>
        <CarouselPrevious className="-left-6"></CarouselPrevious>
        <CarouselNext className="-right-8"></CarouselNext>
      </Carousel>
    </section>
  )
}
