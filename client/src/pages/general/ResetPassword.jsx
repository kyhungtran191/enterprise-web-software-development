import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@radix-ui/react-dropdown-menu'
import React from 'react'
import { Link } from 'react-router-dom'

export default function ResetPassword() {
  return (
    <div className="container flex flex-col items-center justify-center h-screen">
      <div className="md:w-[50%]  min-h-[400px] shadow-2xl  p-10 rounded-lg ">
        <div className="flex items-center justify-center gap-3">
          <img src="./logo.png" alt="" />
          <h2 className='text-xl font-bold md:text-3xl'>Reset Password</h2>
        </div>
        <form action="">
          <div className='my-3'>
            <Label className='my-2 text-xl font-bold md:text-2xl'>New Password</Label>
            <Input className='py-3 text-base font-bold shadow-inner md:text-lg md:py-5' placeholder='Your New Password here'></Input>
            <div className='h-5 mt-3 text-base font-semibold text-red-500'>This is error</div>
          </div>
          <div className='my-3'>
            <Label className='my-2 text-xl font-bold md:text-2xl'>Confirm Password</Label>
            <Input className='py-3 text-base font-bold shadow-inner md:text-lg md:py-5' placeholder='Confirm Password'></Input>
            <div className='h-5 mt-3 text-base font-semibold text-red-500'>This is error</div>
          </div>
          <div className='text-right'>
            <Link to="/login" className='inline-block mt-3 font-semibold text-blue-500 underline cursor-pointer md:mt-5 md:text-base'>Back to Login</Link>
          </div>
          <Button className='w-full my-4 font-bold p-7 md:my-10 md:text-xl md:p-10'>Change New Password</Button>
        </form>
      </div>
    </div>
  )
}
