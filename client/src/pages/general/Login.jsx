import Loading from '@/components/Loading'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Icon } from '@iconify/react'
import React, { useState } from 'react'
import { useForm } from 'react-hook-form'
import { Link } from 'react-router-dom'
import * as yup from 'yup'
export default function Login() {
  const [isOpen, setIsOpen] = useState(false)
  const isLoading = true

  const {
    register,
    handleSubmit,
    control,
    watch,
    formState: { errors }
  } = useForm()

  const onSubmit = data => console.log(data)
  return (
    <div className="fixed grid w-screen h-screen medium:grid-cols-2">
      {/* Left */}
      <img
        src="./thumb.jpg"
        alt="login-thumb"
        className="hidden object-cover w-full h-full lg:block"
      />
      {/* Right */}
      <div className="w-full p-8 medium:py-14 medium:px-12">
        <div className="flex items-center justify-center gap-4">
          <img
            src="./logo.png"
            alt="logo"
            className="flex-shrink-0 object-cover w-10 h-10 sm:h-16 sm:w-16"
          />
          <h1 className="text-lg font-bold text-center sm:text-xl">
            Magazine University System
          </h1>
        </div>
        <h2 className="mt-12 text-xl font-semibold">Nice to see you again!</h2>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="my-4">
            <Label className="text-md">Email</Label>
            <Input
              className="p-4 mt-2 outline-none"
              placeholder="Student Email"
            ></Input>
          </div>
          <div className="my-4">
            <Label className="text-md">Password</Label>
            <div className="relative">
              <Input
                className="p-4 mt-2 outline-none"
                placeholder="Enter Password"
                type={isOpen ? 'text' : 'password'}
              ></Input>
              <div
                className="absolute -translate-y-1/2 top-[50%] w-5 h-5 cursor-pointer right-5"
                onClick={() => setIsOpen(!isOpen)}
              >
                {isOpen ? (
                  <Icon icon="ri:eye-off-fill" className="w-full h-full"></Icon>
                ) : (
                  <Icon icon="mdi:eye" className="w-full h-full"></Icon>
                )}
              </div>
            </div>
          </div>

          <Link to="/" className="inline-block ml-auto text-blue-500 underline">
            Forgot password?
          </Link>
          <Button
            type="submit"
            className={`w-full py-6 mt-8 text-lg transition-all duration-300 ease-in-out bg-blue-600 hover:bg-blue-700 ${isLoading ? 'pointer-events-none bg-opacity-65' : ''}`}
          >
            {isLoading ? <Loading></Loading> : 'Sign In'}
          </Button>
        </form>
      </div>
    </div>
  )
}
