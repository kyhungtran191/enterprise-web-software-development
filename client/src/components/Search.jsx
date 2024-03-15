import { Icon } from '@iconify/react'
import React from 'react'

export default function Search({ className }) {
  return (
    <div className={`flex items-center w-full px-5 py-4 border rounded-lg gap-x-2 ${className}`}>
      <Icon icon="ic:outline-search" className="flex-shrink-0 w-6 h-6 text-slate-700"></Icon>
      <input type="text" className='flex-1 border-none outline-none' placeholder="What you're looking for ?" />
    </div>
  )
}
