import GeneralLayout from '@/layouts'
import { Icon } from '@iconify/react'
import React, { useState } from 'react'
import { Link, useNavigation } from 'react-router-dom'
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
import { Button } from '@/components/ui/button'
export default function Home() {
  const [isLoaded, setIsLoaded] = useState(false);
  const handleLoadedData = () => {
    setIsLoaded(true);
    console.log('Video đã được tải xong!');
  };


  console.log(navigator.state)
  return (<div>
    <div className="container">
      <Search></Search>
    </div>
    <section className='section md:p-0 h-[390px] lg:h-[800px] w-full relative overflow-hidden my-4 bg-blue-700' >
      <div className='absolute inset-0 w-full h-full bg-black/40'></div>
      <div className="grid h-full grid-cols-[1.5fr_1fr]">
        <video
          src='https://res.cloudinary.com/dhaozfpxg/video/upload/v1711681555/greenwich-split_rxhtpj.mp4'
          className='object-cover w-full h-full'
          autoPlay
          loop
          muted
        ></video>
        <div className='z-20 flex flex-col items-center justify-center w-full h-full'>
          <img src="./greenwich-logo.png" alt="logo" className='w-[50%] object-cover flex-shrink-0' />
          <h1
            className='text-2xl text-white md:text-2xl lg:text-[44px] font-semibold my-8'
          >
            Discover Your Creativity
          </h1>
          <Button
            type='button'
            className='text-xl font-semibold duration-300 bg-white p-7 w-max text-darkGrey hover:text-white hover:bg-black'
          >
            Explore your skills
          </Button>
        </div>
      </div>


    </section>
    {/* Search  */}
    <div className="container">

      {/* Faculty */}
      <section className="my-4">
        <h2 className='text-2xl font-bold'>Faculty</h2>
        <Carousel >
          <CarouselContent className="py-4">
            {Array(5).fill(0).map((item, index) => (
              <CarouselItem className="basis-1/2 md:basis-1/3 medium:basis-1/4" key={index}>
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
            <Contributor key={item}></Contributor>
          ))}
        </div>
      </section>
      {/* Featured */}
      <section className="my-4 ">
        <h2 className='my-2 text-2xl font-bold'>Featured Article</h2>
        <div className="grid grid-cols-12 grid-rows-3 gap-4">
          <div className="col-span-12 row-span-3 medium:col-span-6">
            <Article></Article>
          </div>
          <div className="col-span-12 row-span-1 medium:col-span-6">
            <Article isRevert={true} classImageCustom={"md:h-[200px] md:w-[56%]"}></Article>
          </div>
          <div className="col-span-12 row-span-1 medium:col-span-6"> <Article isRevert={true} classImageCustom={"md:h-[200px] md:w-[56%]"}></Article></div>
          <div className="col-span-12 row-span-1 medium:col-span-6"> <Article isRevert={true} classImageCustom={"md:h-[200px] md:w-[56%]"}></Article></div>
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

  </div>)


}
