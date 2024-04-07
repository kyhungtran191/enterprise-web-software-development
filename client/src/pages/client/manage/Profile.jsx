import DatePickerCustom from '@/components/DatePickerCustom';
import DateSelect from '@/components/DateSelect';
import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import { Input } from '@/components/ui/input';
import { STUDENT_OPTIONS } from '@/constant/menuSidebar'
import AdminLayout from '@/layouts/AdminLayout'
import { Label } from '@radix-ui/react-dropdown-menu';
import React, { useEffect, useState } from 'react'
import { useDropzone } from 'react-dropzone';
import { useQuery } from "@tanstack/react-query"
import { Auth } from '@/services/client';
import { toast } from 'react-toastify'
import * as yup from 'yup'
import { yupResolver } from '@hookform/resolvers/yup'
import { useForm, Controller } from 'react-hook-form';
import ActionSpinner from '@/components/ActionSpinner';
import Spinner from '@/components/Spinner';
import { formatDate } from '@/utils/helper';
import { differenceInYears, parse } from 'date-fns';
export default function Profile() {
  const [date, setDate] = useState()
  const [currentThumbnail, setCurrentThumbnail] = useState()
  const schema = yup
    .object({
      email: yup.string(),
      firstName: yup.string().required('Please provide first name'),
      lastName: yup.string().required('Please provide first name'),
      phoneNumber: yup.string(),
      faculty: yup.string()
    })
    .required()
  const {
    handleSubmit,
    control,
    reset,
    formState: { errors }
  } = useForm({ resolver: yupResolver(schema) })
  const handleChangeImage = (e) => {
    const file = e.target.files[0]
    if (file) {
      let url = URL.createObjectURL(file)
      setCurrentThumbnail(url)
    }
  }

  const { isLoading, data } = useQuery({
    queryKey: ['profile'],
    queryFn: Auth.profile
  })
  let detailData = data && data?.data?.responseData
  useEffect(() => {
    reset({
      email: detailData?.email,
      firstName: detailData?.firstName,
      lastName: detailData?.lastName,
      phoneNumber: detailData?.phoneNumber ?? '',
      faculty: detailData?.faculty
    })
  }, [detailData, reset])




  return (
    <AdminLayout links={STUDENT_OPTIONS}>
      {isLoading && <div className="flex items-center justify-center min-h-screen"><Spinner className={"border-black"}></Spinner></div>}
      <div className=" sm:p-10 rounded-lg shadow-sm min-h-[80vh]">
        <label htmlFor="image" className='cursor-pointer '>
          <img src={currentThumbnail ? currentThumbnail : "../../user.jpg"} alt="" className='w-[80px] h-[80px] rounded-full shadow-lg border-black mx-auto hover:bg-black object-cover' />
          <input type="file" className="hidden" id="image" onChange={handleChangeImage} />
          <div className='mt-5 font-semibold text-center'>Avatar</div>
        </label>
        <form className="grid items-start justify-center grid-cols-2 gap-5">
          <div className='col-span-2 sm:col-span-1'>
            <Label className='font-semibold text-md'>First Name</Label>
            <Controller
              control={control}
              name="firstName"
              render={({ field }) => (
                <Input
                  className='p-4 mt-2 font-semibold shadow-inner outline-none '
                  placeholder='Enter First Name'
                  type={'text'}
                  {...field}
                ></Input>
              )}
            />

            {/* <div className='h-5 mt-3 text-base font-semibold text-red-500'>{errors && errors?.password?.message}</div> */}
          </div>
          <div className='col-span-2 sm:col-span-1'>
            <Label className='font-semibold text-md'>Last Name</Label>
            <Controller
              control={control}
              name="lastName"
              render={({ field }) => (
                <Input
                  className='p-4 mt-2 font-semibold shadow-inner outline-none '
                  placeholder='Enter Last Name'
                  type={'text'}
                  {...field}
                ></Input>
              )}
            />
            {/* <div className='h-5 mt-3 text-base font-semibold text-red-500'>{errors && errors?.password?.message}</div> */}
          </div>
          <div className='col-span-2 sm:col-span-1'>
            <Label className='font-semibold text-md'>Email</Label>
            <Controller
              control={control}
              name="email"
              render={({ field }) => (
                <Input
                  className='p-4 mt-2 font-semibold bg-gray-300 shadow-inner outline-none'
                  placeholder='Enter Email'
                  disabled
                  type={'text'}
                  {...field}
                ></Input>
              )}
            />
            {/* <div className='h-5 mt-3 text-base font-semibold text-red-500'>{errors && errors?.password?.message}</div> */}
          </div>
          <div className='col-span-2 sm:col-span-1'>
            <Label className='font-semibold text-md'>Phone</Label>
            <Controller
              control={control}
              name="phoneNumber"
              render={({ field }) => (
                <Input
                  className='p-4 mt-2 font-semibold shadow-inner outline-none '
                  placeholder='+84'
                  type={'text'}
                  {...field}
                ></Input>
              )}
            />
            {/* <div className='h-5 mt-3 text-base font-semibold text-red-500'>{errors && errors?.password?.message}</div> */}
          </div>
          <div className='col-span-2 sm:col-span-1'>
            <Label className='font-semibold text-md'>Faculty</Label>
            <Controller
              control={control}
              name="faculty"
              render={({ field }) => (
                <Input
                  className='p-4 mt-2 font-semibold bg-gray-300 shadow-inner outline-none'
                  placeholder='Faculty'
                  type={'text'}
                  disabled
                  {...field}
                ></Input>
              )}
            />
            {/* <div className='h-5 mt-3 text-base font-semibold text-red-500'>{errors && errors?.password?.message}</div> */}
          </div>
          <div className='col-span-2 sm:col-span-1'>
            <Label className='font-semibold text-md'>Date of Birth {date && `(${formatDate(date)})`}</Label>
            {/* <div className='h-5 mt-3 text-base font-semibold text-red-500'>{errors && errors?.password?.message}</div> */}
            <DatePickerCustom mode="single"
              captionLayout="dropdown-buttons"
              selected={date}
              onSelect={setDate}
              fromYear={1960}
              toYear={2030}></DatePickerCustom>
          </div>
          <Button className="col-span-2 py-6 bg-blue-500">Update</Button>
        </form>
      </div>
    </AdminLayout>
  )
}
