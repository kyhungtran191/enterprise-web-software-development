import React from 'react'
import Search from '@/components/Search'
import { Button } from '@/components/ui/button'
import Faculty from './Home/partials/Faculty'
import TopContributors from './Home/partials/TopContributors'
import FeaturedContribution from './Home/partials/FeaturedContribution'
import LatestContribution from './Home/partials/LatestContribution'
import GuestContribution from './Home/partials/guestContribution'
export default function Home() {

  return (<div>
    <div className="container">
      {/* Search  */}
      <Search></Search>
    </div>
    <section className='section md:p-0 h-[390px] lg:h-[800px] w-full relative overflow-hidden my-4 bg-blue-700' >
      <div className='absolute inset-0 w-full h-full bg-black/40'></div>
      <div className="grid h-full grid-cols-1 md:grid-cols-[1.5fr_1fr]">
        <video
          src='https://res.cloudinary.com/dhaozfpxg/video/upload/v1711681555/greenwich-split_rxhtpj.mp4'
          className='object-cover w-full h-full'
          autoPlay
          loop
          muted
        ></video>
        <div className='z-20 flex-col items-center justify-center hidden w-full h-full md:flex'>
          <img src="./greenwich-logo.png" alt="logo" className='w-[50%] object-cover flex-shrink-0' />
          <h1
            className='my-8 text-2xl font-semibold text-center text-white md:text-2xl'
          >
            Discover Your Creativity
          </h1>
          <Button
            type='button'
            className='p-5 text-xl font-semibold duration-300 bg-white w-max text-darkGrey hover:text-white hover:bg-black'
          >
            Explore your skills
          </Button>
        </div>
      </div>
    </section>
    <div className="container">
      {/* Faculty */}
      <Faculty></Faculty>
      {/* Contributor */}
      <TopContributors></TopContributors>
      {/* Featured */}
      <FeaturedContribution></FeaturedContribution>
      {/* Latest */}
      <LatestContribution></LatestContribution>
      <GuestContribution></GuestContribution>
    </div>

  </div>)


}
