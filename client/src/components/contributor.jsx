import React from 'react'

export default function Contributor() {
  return (
    <div className="flex items-center w-full gap-x-8">
      <img src="https://variety.com/wp-content/uploads/2021/04/Avatar.jpg" alt="" className="object-cover w-10 h-10 rounded-full flex-shirk-0" />
      <div className='flex-1'>
        <h1 className='font-semibold'>Tran Ky Hung</h1>
        <p className='text-ellipsis line-clamp-1'>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Esse, exercitationem porro. Repudiandae quia non tempore quibusdam, incidunt consequuntur molestias laudantium minus dicta corporis hic perspiciatis consectetur sunt ratione in voluptas.</p>
      </div>
    </div>
  )
}
