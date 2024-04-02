import React from 'react'

export default function Contributor({ info }) {
  return (
    <div className="flex items-center w-full gap-x-8">
      <img src={`${info?.avatar ? info?.avatar : "https://cdn-icons-png.flaticon.com/512/6596/6596121.png"}`} alt="" className="object-cover w-10 h-10 rounded-full flex-shirk-0" />
      <div className='flex-1'>
        <h1 className='font-semibold'>{info?.userName}</h1>
        <p className='font-semibold text-ellipsis line-clamp-1'>Contribution: {info.contributionCount}</p>
      </div>
    </div>
  )
}
