import { Button } from '@/components/ui/button'
import React from 'react'
import { Link } from 'react-router-dom'

export default function NotFound() {
  return (
    <div className="fixed inset-0 flex flex-col items-center justify-center h-screen">
      <img src="./404.png" alt="not-found" className="w-[600px] object-cover flex-shrink-0" />
      <Button className="p-10 text-2xl text-white bg-blue-600">
        <Link to="/" className='flex items-center justify-center w-full h-full'>Go back to Homepage</Link>
      </Button>
    </div>
  )
}
