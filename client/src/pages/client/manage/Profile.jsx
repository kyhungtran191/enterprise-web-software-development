import DatePickerCustom from '@/components/DatePickerCustom';
import DateSelect from '@/components/DateSelect';
import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import { Input } from '@/components/ui/input';
import { ADMIN_OPTIONS, STUDENT_OPTIONS } from '@/constant/menuSidebar'
import AdminLayout from '@/layouts/AdminLayout'
import { Label } from '@radix-ui/react-dropdown-menu';
import React, { useEffect, useState } from 'react'
import { useDropzone } from 'react-dropzone';
import { useMutation, useQuery } from "@tanstack/react-query"
import { Auth } from '@/services/client';
import { toast } from 'react-toastify'
import * as yup from 'yup'
import { yupResolver } from '@hookform/resolvers/yup'
import { useForm, Controller } from 'react-hook-form';
import Spinner from '@/components/Spinner';
import { formatDate } from '@/utils/helper';
import { PHONE_REG } from '@/utils/regex';
import { format } from 'date-fns';
import ActionSpinner from '@/components/ActionSpinner';
import { useAppContext } from '@/hooks/useAppContext';
import { Roles } from '@/constant/roles';
export default function Profile() {
  const [date, setDate] = useState()
  const [currentThumbnail, setCurrentThumbnail] = useState()
  const updateMutation = useMutation({
    mutationFn: (body) => Auth.updateProfile(body)
  })
  const schema = yup
    .object({
      email: yup.string(),
      firstName: yup.string().required('Please provide first name'),
      lastName: yup.string().required('Please provide last name'),
      phoneNumber: yup.string().matches(PHONE_REG, 'Please provide correct phone format'),
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
    const isoString = detailData?.dob
    const dateObject = new Date(isoString)
    setDate(dateObject)
  }, [detailData, reset])


  const dateString = date;

  const onSubmit = (data) => {
    // TransferData
    const formData = new FormData();
    formData.append("FirstName", data.firstName)
    formData.append("LastName", data.lastName)
    if (data.phoneNumber) {
      formData.append("PhoneNumber", data.phoneNumber)
    }
    if (date) {
      const dateObject = dateString && new Date(dateString);
      const utcDateObject = dateObject && new Date(dateObject.getTime() - (dateObject.getTimezoneOffset() * 60000));
      const isoString = utcDateObject && utcDateObject?.toISOString();
      formData.append("Dob", isoString)
    }
    if (currentThumbnail?.startsWith('blob:')) {
      fetch(currentThumbnail)
        .then(response => response.blob())
        .then(blob => {
          const fileName = 'avatar';
          const file = new File([blob], fileName);
          console.log(1)
          file && formData.append("Avatar", file)
        })
        .catch(error => {
          console.error('Error when load Blob', error);
        });
    }
    updateMutation.mutate(formData, {
      onSuccess(data) {
        toast.success("Update Profile Successfully!")
      },
      onError(err) {
        console.log(err)
      }
    })
  }
  const { profile } = useAppContext()

  return (
    <AdminLayout links={STUDENT_OPTIONS}>
      {isLoading || updateMutation.isLoading && <ActionSpinner></ActionSpinner>}
      <div className="sm:p-10 rounded-lg shadow-2xl min-h-[80vh]">
        <label htmlFor="image" className='mx-auto cursor-pointer '>
          <img src={currentThumbnail ? currentThumbnail : "../../user.jpg"} alt="" className='w-[80px] h-[80px] rounded-full shadow-lg border-black mx-auto hover:bg-black object-cover' />
          <input type="file" className="hidden" id="image" onChange={handleChangeImage} />
          <div className='mt-5 font-semibold text-center'>Avatar</div>
        </label>
        <form className="grid items-start justify-center grid-cols-2 gap-5" onSubmit={handleSubmit(onSubmit)}>
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

            <div className='h-5 mt-3 text-base font-semibold text-red-500'>{errors && errors?.firstName?.message}</div>
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
            <div className='h-5 mt-3 text-base font-semibold text-red-500'>{errors && errors?.lastName?.message}</div>
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
            <div className='h-5 mt-3 text-base font-semibold text-red-500'>{errors && errors?.phoneNumber?.message}</div>
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
          </div>
          <div className='col-span-2 sm:col-span-1'>
            <Label className='font-semibold text-md'>Date of Birth {date && `(${formatDate(date)})`}</Label>
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
