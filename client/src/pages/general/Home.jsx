import GeneralLayout from '@/layouts'
import { Icon } from '@iconify/react'
import React from 'react'
import { Link } from 'react-router-dom'
import Autoplay from "embla-carousel-autoplay"
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from "@/components/ui/carousel"
import Contributor from '@/components/contributor'
import Article from '@/components/article'
import Search from '@/components/Search'
export default function Home() {
  return <GeneralLayout>
    <div className="container">
      {/* Search  */}
      <Search></Search>
      {/* Faculty */}
      <section className="my-4">
        <h2 className='text-2xl font-bold'>Faculty</h2>
        <Carousel >
          <CarouselContent className="py-4">
            {Array(5).fill(0).map((item) => (
              <CarouselItem className="basis-1/2 md:basis-1/3 medium:basis-1/4">
                <Link to="" className="h-[180px] md:h-[234px] flex flex-col items-center justify-center gap-3 transition duration-300 ease-in-out rounded-md shadow-lg hover:-translate-y-2">
                  <img src="./falcuty-icon.png" alt="" className='w-[32px] h-[32px] flex-shrink-0' />
                  <h3 className='font-semibold'>Marketing</h3>
                </Link>
              </CarouselItem>
            ))}
          </CarouselContent>
          <CarouselPrevious className="-left-6"></CarouselPrevious>
          <CarouselNext className="-right-8"></CarouselNext>
        </Carousel>
      </section>
      {/* Contributor */}
      <section className="hidden my-4 sm:block">
        <h2 className='text-2xl font-bold'>Top Contributor</h2>
        <div className="grid grid-cols-2 gap-10 my-10 medium:grid-cols-3">
          {Array(6).fill(0).map((item) => (
            <Contributor></Contributor>
          ))}
        </div>
      </section>
      {/* Featured */}
      <section className="my-4 ">
        <h2 className='my-2 text-2xl font-bold'>Featured Article</h2>
        <div class="grid grid-rows-3 grid-cols-12 gap-4">
          <div class="row-span-3 col-span-12 medium:col-span-7">
            <Article></Article>
          </div>
          <div class="row-span-1 col-span-12 medium:col-span-5">
            <Article isRevert={true}></Article>
          </div>
          <div class="row-span-1 col-span-12 medium:col-span-5"> <Article isRevert={true}></Article></div>
          <div class="row-span-1 col-span-12 medium:col-span-5"> <Article isRevert={true}></Article></div>
        </div>
      </section>

      <section className="my-4">
        <div className="flex items-center justify-between">
          <h2 className='my-2 text-2xl font-bold'>Latest Article</h2>
          <Link to="" className='underline text-slate-600'>View More</Link>
        </div>
        <div className="grid gap-3 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 xl:gap-4">
          <Article></Article>
          <Article></Article>
          <Article></Article>
          <Article></Article>
        </div>
      </section>
    </div>
  </GeneralLayout>
}
