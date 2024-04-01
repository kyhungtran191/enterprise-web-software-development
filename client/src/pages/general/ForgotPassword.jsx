import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@radix-ui/react-dropdown-menu'
import React from 'react'
import { Link } from 'react-router-dom'

export default function ForgotPassword() {
  return (
    <div className="container flex flex-col items-center justify-center h-screen">
      <div className="w-[50%]  min-h-[400px] shadow-2xl  p-10 rounded-lg ">
        <div className="flex items-center justify-center gap-3">
          <img src="./logo.png" alt="" />
          <h2 className='text-3xl font-bold'>Recover Password</h2>
        </div>
        <form action="">
          <Label className='my-4 text-2xl font-bold'>Your Email</Label>
          <Input className='py-5 text-lg font-bold shadow-inner' placeholder='Your Email here'></Input>
          <div className='h-5 mt-3 text-base font-semibold text-red-500'>This is error</div>
          <div className='text-right'>
            <Link to="/login" className='inline-block mt-5 font-semibold text-right text-blue-500 underline cursor-pointer '>Back to Login</Link>
          </div>
          <Button className='w-full p-10 my-10 text-xl font-bold '>Send Verify Email</Button>
        </form>

      </div>
    </div>
  )
}
