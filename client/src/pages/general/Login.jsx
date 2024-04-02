import Loading from '@/components/Loading'
import Spinner from '@/components/Spinner'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Auth } from '@/services/client'
import { Icon } from '@iconify/react'
import { useMutation } from '@tanstack/react-query'
import React, { useState } from 'react'
import { Controller, useForm } from 'react-hook-form'
import { Link, useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import * as yup from 'yup'
export default function Login() {
  const [isOpen, setIsOpen] = useState(false)
  const navigate = useNavigate()
  const {
    register,
    handleSubmit,
    control,
    watch,
    formState: { errors }
  } = useForm()
  const { isLoading, mutate } = useMutation({
    mutationFn: (body) => Auth.login(body),
  })
  const onSubmit = (data) => {
    mutate(data, {
      onSuccess(data) {
        if (data.data.accessToken && data.data.refreshToken) {
          toast.success("Login Successfully!")
          navigate('/')
        }
      },
      onError(data) {
        const errorMessage = data && data?.response?.data?.title
        toast.error(errorMessage)
      }
    })
  }
  return (
    <div className='fixed grid w-screen h-screen medium:grid-cols-2'>
      {/* Left */}
      <img
        src='./thumb.jpg'
        alt='login-thumb'
        className='hidden object-cover w-full h-full lg:block'
      />
      {/* Right */}
      <div className='w-full p-8 medium:py-14 medium:px-12'>
        <div className='flex items-center justify-center gap-4'>
          <img
            src='./logo.png'
            alt='logo'
            className='flex-shrink-0 object-cover w-10 h-10 sm:h-16 sm:w-16'
          />
          <h1 className='text-lg font-bold text-center sm:text-xl'>
            Magazine University System
          </h1>
        </div>
        <h2 className='mt-12 text-xl font-semibold'>Nice to see you again!</h2>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className='my-4'>
            <Label className='text-md'>Email</Label>
            <Controller
              control={control}
              name="email"
              render={({ field }) => (
                <Input
                  className='p-4 mt-2 outline-none'
                  placeholder='Student Email'
                  {...field}
                ></Input>
              )}
            />
          </div>
          <div className='my-4'>
            <Label className='text-md'>Password</Label>
            <div className='relative'>
              <Controller
                control={control}
                name="password"
                render={({ field }) => (
                  <Input
                    className='p-4 mt-2 outline-none'
                    placeholder='Enter Password'
                    type={isOpen ? 'text' : 'password'}
                    {...field}
                  ></Input>
                )}
              />
              <div
                className='absolute -translate-y-1/2 top-[50%] w-5 h-5 cursor-pointer right-5'
                onClick={() => setIsOpen(!isOpen)}
              >
                {isOpen ? (
                  <Icon icon='ri:eye-off-fill' className='w-full h-full'></Icon>
                ) : (
                  <Icon icon='mdi:eye' className='w-full h-full'></Icon>
                )}
              </div>
            </div>
          </div>

          <Link to='/forgot-password' className='inline-block ml-auto text-blue-500 underline'>
            Forgot password?
          </Link>
          <Button
            type='submit'
            className={`w-full py-6 mt-8 text-lg transition-all duration-300 ease-in-out bg-blue-600 hover:bg-blue-700 ${isLoading ? 'pointer-events-none bg-opacity-65' : ''}`}
          >

            {isLoading ? <Spinner></Spinner> : "Sign In"}
          </Button>
        </form>
      </div>
    </div>
  )
}
