import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@radix-ui/react-dropdown-menu'
import React, { useState } from 'react'
import { Controller, useForm } from 'react-hook-form'
import { Link } from 'react-router-dom'
import * as yup from "yup"
import { yupResolver } from "@hookform/resolvers/yup"
import { EMAIL_REG } from '@/utils/regex'
import { useMutation } from '@tanstack/react-query'
import { Auth } from '@/services/client'
import Spinner from '@/components/Spinner'
export default function ForgotPassword() {
  const [isSuccess, setIsSuccess] = useState(false);
  const schema = yup
    .object({
      email: yup.string().matches(EMAIL_REG, 'Please provide correct email type').required('Please provide your email'),
    })
    .required()
  const { handleSubmit, control, formState: { errors }, setError } = useForm({
    resolver: yupResolver(schema)
  })
  const { mutate, isLoading } = useMutation({
    mutationFn: (email) => Auth.forgotPassword(email)
  })

  const onSubmit = (data) => {
    mutate(data.email, {
      onSuccess() {
        setIsSuccess(true)
      },
      onError(data) {
        const errorMessage = data && data?.response?.data?.title
        if (errorMessage) {
          setError('email', { message: errorMessage })
        }
      }
    })
  }

  return (
    <div className="container flex flex-col items-center justify-center h-screen">
      <div className="md:w-[50%]  min-h-[400px] shadow-2xl  p-10 rounded-lg ">
        <div className="flex items-center justify-center gap-3">
          <img src="./logo.png" alt="" />
          <h2 className='text-xl font-bold md:text-3xl'>Recover Password</h2>
        </div>
        {!isLoading && !isSuccess && <form onSubmit={handleSubmit(onSubmit)}>
          <Label className='my-4 text-xl font-bold md:text-2xl'>Your Email</Label>
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
          <div className='h-5 mt-3 text-base font-semibold text-red-500'>{errors && errors?.email?.message}</div>
          <div className='text-right'>
            <Link to="/login" className='inline-block mt-3 font-semibold text-blue-500 underline cursor-pointer md:mt-5 md:text-base'>Back to Login</Link>
          </div>
          <Button type={"submit"} className='w-full my-4 font-bold p-7 md:my-10 md:text-xl md:p-10'>Send Verify Email</Button>
        </form>}
        {isLoading && <div className="flex items-center justify-center w-full h-full">
          <Spinner></Spinner>
        </div>}
        {isSuccess && (<>
          <div className="mt-4 font-semibold text-center text-green-500">Success! Please check your email for further instructions.</div>
          <div className='mt-10 text-center'>
            <Link to="/login">
              <Button className="bg-blue-500 btn btn-primary" >Back to Login</Button>
            </Link>
          </div>
        </>
        )}
      </div>
    </div >
  )
}
