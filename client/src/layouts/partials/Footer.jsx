import { path } from '@/configs'
import React from 'react'
import { Link } from 'react-router-dom'

export default function Footer() {
  return (
    <footer className='py-[82px] bg-black text-white mt-auto relative z-40'>
      <div className='container'>
        <div className='flex flex-wrap items-start justify-between'>
          <div className='flex flex-col mb-4 w-full md:max-w-[258px] md:mb-0'>
            <Link to={path.home} className='flex items-center gap-x-3'>
              <h2 className='text-2xl font-semibold font-title '>Greenwich University</h2>
            </Link>
            <p className='mt-4 text-sm'>
              <span className='font-semibold'>Address:</span> 20 Cong Hoa
            </p>
            <p className='mt-4 text-sm font-semibold '></p>
            <p className='mt-4 text-sm '>trankyhung225@gmail.com</p>
          </div>
          <div className='hidden flex-col items-start  max-w-[258px] md:flex'>
            <h3 className='text-xl font-semibold'>Contribution</h3>
            {/* {menuOptions.map((option, index) => (
              <Link
                to={option.path}
                className='py-2 text-base font-normal transition duration-200 hover:text-black24 hover:font-medium'
                key={index}
              >
                {option.title}
              </Link>
            ))} */}
          </div>
          <div className='max-w-[586px]'>
            <h3 className='text-lg font-semibold md:text-4xl'>Get information</h3>
            <p className='mt-4 text-sm md:text-base'>
              Enter your email below to be the first to know about new collections and product launches.
            </p>
            <div className='flex items-center h-[48px] mt-11'>
              <input
                className='flex-1 h-full px-5 py-4 border-none outline-none focus:outline-none'
                type='text'
                placeholder='Nhập địa chỉ email'
              />
              <button className='h-full px-6 text-xs font-semibold text-white bg-black sm:text-base'>Follow</button>
            </div>
          </div>
        </div>
      </div>
    </footer>
  )
}
