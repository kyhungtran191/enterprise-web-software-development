import React from 'react'

export default function Loading() {
  return (
    <div className='w-10 h-10 p-1 rounded-full p animate-spin drop-shadow-md bg-gradient-to-bl from-pink-400 via-purple-400 to-indigo-600 md:w-10 md:h-10 aspect-square'>
      <div className='w-full h-full bg-white rounded-full dark:bg-zinc-900 background-blur-md'></div>
    </div>
  )
}
