import Spinner from '@/components/Spinner'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@radix-ui/react-dropdown-menu'
import React, { useEffect, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import * as yup from "yup"
import { yupResolver } from "@hookform/resolvers/yup"
import { Controller, useForm } from 'react-hook-form'
import { Icon } from '@iconify/react'
import { useMutation } from '@tanstack/react-query'
import { Auth } from '@/services/client'
import { toast } from 'react-toastify'
import { PASSWORD_REG } from '@/utils/regex'
export default function ResetPassword() {
  const navigate = useNavigate()
  const { token } = useParams();
  const [isConfirmed, setIsConfirmed] = useState(false)
  const [isOpenPassword, setIsOpenPassword] = useState(false)
  const [isOpenConfirm, setIsOpenConfirm] = useState(false)
  const schema = yup
    .object({
      password: yup.string().matches(PASSWORD_REG, { message: "The password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character" }).required('Please provide password'),
      confirm_password: yup.string().required("Please provide confirmation password").oneOf([yup.ref('password')], "Confirm password does not match"),
    })
    .required()
  const { handleSubmit, control, formState: { errors }, setError } = useForm({
    resolver: yupResolver(schema)
  })
  const { mutate } = useMutation({
    mutationFn: (token) => Auth.validateToken(token)
  })
  useEffect(() => {
    mutate(token, {
      onSuccess() {
        setIsConfirmed(true)
      },
      onError(err) {
        let errMessage = err && err?.response?.data?.title
        toast.error(errMessage)
        navigate('/forgot-password')
      }
    })
  }, [token])

  const resetPasswordMutation = useMutation({
    mutationFn: (body) => Auth.resetPassword(body)
  })
  const onSubmit = (data) => {
    resetPasswordMutation.mutate({ password: data.password, token }, {
      onSuccess() {
        toast.success("Reset Password Successfully! Please login again")
        navigate('/login')
      },
      onError(err) {
        console.log(err)
      }
    })
  }
  return (
    <div className="container flex flex-col items-center justify-center h-screen">
      {isConfirmed && <div className="md:w-[50%]  min-h-[400px] shadow-2xl  p-10 rounded-lg ">
        <div className="flex items-center justify-center gap-3">
          <img src="./logo.png" alt="" />
          <h2 className='text-xl font-bold md:text-3xl'>Reset Password</h2>
        </div>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className='my-3'>
            <Label className='my-2 text-xl font-bold md:text-2xl'>New Password</Label>
            <div className='relative'>
              <Controller
                control={control}
                name="password"
                render={({ field }) => (
                  <Input
                    className='p-4 mt-2 outline-none'
                    placeholder='Enter Password'
                    type={isOpenPassword ? 'text' : 'password'}
                    {...field}
                  ></Input>
                )}
              />
              <div
                className='absolute -translate-y-1/2 top-[50%] w-5 h-5 cursor-pointer right-5'
                onClick={() => setIsOpenPassword(!isOpenPassword)}
              >
                {isOpenPassword ? (
                  <Icon icon='ri:eye-off-fill' className='w-full h-full'></Icon>
                ) : (
                  <Icon icon='mdi:eye' className='w-full h-full'></Icon>
                )}
              </div>
            </div>

            <div className='mt-3 text-base font-semibold text-red-500 '>{errors && errors?.password?.message}</div>
          </div>
          <div className='my-3'>
            <Label className='my-2 text-xl font-bold md:text-2xl'>Confirm Password</Label>
            <div className='relative'>
              <Controller
                control={control}
                name="confirm_password"
                render={({ field }) => (
                  <Input
                    className='p-4 mt-2 outline-none'
                    placeholder='Enter Confirm Password'
                    type={isOpenConfirm ? 'text' : 'password'}
                    {...field}
                  ></Input>
                )}
              />
              <div
                className='absolute -translate-y-1/2 top-[50%] w-5 h-5 cursor-pointer right-5'
                onClick={() => setIsOpenConfirm(!isOpenConfirm)}
              >
                {isOpenConfirm ? (
                  <Icon icon='ri:eye-off-fill' className='w-full h-full'></Icon>
                ) : (
                  <Icon icon='mdi:eye' className='w-full h-full'></Icon>
                )}
              </div>
            </div>
            <div className='h-5 mt-3 text-base font-semibold text-red-500'>{errors && errors?.confirm_password?.message}</div>
          </div>
          <div className='text-right'>
            <Link to="/login" className='inline-block mt-3 font-semibold text-blue-500 underline cursor-pointer md:mt-5 md:text-base'>Back to Login</Link>
          </div>
          <Button className={`w-full my-4 font-bold p-7 md:my-10 md:text-xl md:p-10`}>{resetPasswordMutation.isLoading ? <Spinner></Spinner> : "Change New Password"}</Button>
        </form>
      </div>}
      {!isConfirmed && <Spinner></Spinner>}
    </div>
  )
}
