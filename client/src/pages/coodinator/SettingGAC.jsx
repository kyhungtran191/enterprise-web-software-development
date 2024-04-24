import Search from '@/components/Search'
import { Button } from '@/components/ui/button'
import AdminLayout from '@/layouts/AdminLayout'
import { ArrowDown10Icon, Plus } from 'lucide-react'
import React, { useEffect, useState } from 'react'
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'

import Article from '@/components/article'
import { Link, createSearchParams, useNavigate } from 'react-router-dom'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import useParamsVariables from '@/hooks/useParams'
import { isUndefined, omitBy, omit, debounce } from 'lodash'
import { Icon } from '@iconify/react'
import { Checkbox } from '@/components/ui/checkbox'
import { ArrowUpDown } from 'lucide-react'

import Spinner from '@/components/Spinner'
import { MC_OPTIONS, STUDENT_OPTIONS } from '@/constant/menuSidebar'
import { Contributions } from '@/services/coodinator'
import { CustomTable } from '@/components/CustomTable'
import { formatDate } from '@/utils/helper'

import Swal from 'sweetalert2'
import { toast } from 'react-toastify'
import { Controller, useForm } from 'react-hook-form'
import { yupResolver } from '@hookform/resolvers/yup'
import { EMAIL_REG } from '@/utils/regex'
import * as yup from "yup"
import { Label } from '@/components/ui/label'
import { Input } from '@/components/ui/input'
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
export default function SettingGAC() {
  const navigate = useNavigate()
  const queryParams = useParamsVariables()
  const [inputValue, setInputValue] = useState('')
  const [tableData, setTableData] = useState([])


  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isOpen, setIsOpen] = useState(false)
  const closeDialog = () => setIsOpen(false)




  const handleChangeCheckbox = (data) => {
    setTableData((prev) => prev.map((item) => item.id == data.id ? { ...item, guestAllowed: !item.guestAllowed } : item))
  }
  const handleSelectAll = (value) => {
    setTableData((prev) => prev.map((item) => ({ ...item, guestAllowed: value })))
  }
  const columns = [
    {
      id: 'select',
      header: ({ table }) => (
        <Checkbox
          checked={
            table.getIsAllPageRowsSelected() ||
            (table.getIsSomePageRowsSelected() && 'indeterminate')
          }
          onCheckedChange={(value) => {
            table.toggleAllPageRowsSelected(!!value)
            handleSelectAll(!!value)
          }}
          aria-label='Select all'
          className='mx-4'
        />
      ),
      cell: ({ row }) => (
        <Checkbox
          checked={row.original.guestAllowed}
          onCheckedChange={(value) => {
            row.toggleSelected(!!value)
            handleChangeCheckbox(row.original)
          }
          }
          aria-label='Select row'
        />
      ),
      enableSorting: false,
      enableHiding: false
    },
    {
      accessorKey: 'title',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Contribution Title
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },

    {
      accessorKey: 'shortDescription',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Short Description
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: `publicDate`,
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Date
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'files.length',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Files
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    }
  ]
  const queryConfig = omitBy(
    {
      pageindex: queryParams.pageindex || '1',
      facultyname: queryParams.facultyname,
      status: 'APPROVE',
      keyword: queryParams.keyword,
      name: queryParams.name,
      year: queryParams.year,
      pagesize: queryParams.pagesize
    },
    isUndefined
  )
  const { data, isLoading } = useQuery({
    queryKey: ['mc-contributions', queryConfig],
    queryFn: (_) => Contributions.MCContribution(queryConfig)
  })

  const schema = yup
    .object({
      email: yup
        .string()
        .matches(EMAIL_REG, 'Please provide correct email type')
        .required('Please provide your email'),
      username: yup.string().required('Please provide username')
    })
    .required()
  const {
    handleSubmit,
    control,
    formState: { errors },
    reset
  } = useForm({ resolver: yupResolver(schema) })

  const addNewGuestMutation = useMutation({
    mutationFn: (body) => Contributions.MCCreateGuest(body)
  })

  const { mutate } = useMutation({
    mutationFn: (body) => Contributions.MCAllowGuest(body)
  })

  // let results = currentData?.results?.map((item) => ({ ...item, publicDate: formatDate(item.publicDate) }))
  useEffect(() => {
    setTableData(data && data?.data?.responseData?.results?.map((item) => ({ ...item, publicDate: formatDate(item.publicDate) })) || []);
  }, [data])
  const [selectedRow, setSelectedRow] = useState({})
  const handleApproveArticle = () => {
    let ids = []
    tableData && tableData?.forEach((item) => {
      if (item.guestAllowed) {
        ids.push(item.id)
      }
    });

    Swal.fire({
      title: "Are you sure?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, Approve it!"
    }).then((result) => {
      if (result.isConfirmed) {
        mutate({ ids }, {
          onSuccess() {
            toast.success("Update Contribution for Guest Successfully!")
          },
          onError(data) {
            const errorMessage = data && data?.response?.data?.title
            toast.error(errorMessage)
          }
        })
      }
    });
  }
  const onAddNewGuest = (data) => {
    console.log(data)
    const formData = new FormData();
    formData.append('Email', data?.email);
    formData.append('UserName', data?.username);
    // If IsActive is boolean
    formData.append('IsActive', true);
    addNewGuestMutation.mutate(formData, {
      onSuccess(data) {
        toast.success("Add new guest account successfully!")
        setIsOpen(false)
        reset({
          email: "",
          username: "",
        })
      },
      onError(data) {
        console.log(data)
        const errorMessage = data && data?.response?.data?.title
        toast.error(errorMessage)
      }
    })
  }
  return (
    <AdminLayout links={MC_OPTIONS}>
      <Tabs defaultValue="contributions" className="">
        <TabsList>
          <TabsTrigger value="contributions">Allow Contribution</TabsTrigger>
          <TabsTrigger value="guest">Faculty Guest List</TabsTrigger>
        </TabsList>
        <TabsContent value="contributions" className="w-ful">
          {!isLoading && (
            <div className="max-h-[70vh] overflow-auto">
              <CustomTable
                columns={columns}
                data={tableData}
                path={'/coodinator-manage/setting-guest'}
                // queryConfig={queryConfig}
                // pageCount={data.data?.responseData?.pageCount}
                selectedRows={setSelectedRow}
              ></CustomTable>
            </div>
          )}
          {isLoading && (
            <div className='flex justify-center min-h-screen mt-10'>
              <Spinner></Spinner>
            </div>
          )}

          {!isLoading && tableData?.length < 0 && (
            <div className='my-10 text-3xl font-semibold text-center '>No Data</div>
          )}
          <div className='flex flex-wrap items-center gap-3 my-5'>
            {/* <div
          className={`flex items-center px-5 py-4 border rounded-lg gap-x-2 w-[50vw]`}
        >
          <Icon
            icon='ic:outline-search'
            className='flex-shrink-0 w-6 h-6 text-slate-700'
          ></Icon>
          <input
            type='text'
            className='flex-1 border-none outline-none'
            placeholder="What you're looking for ?"
            defaultValue={queryParams['keyword']}
            onChange={(e) => {
              setInputValue(e.target.value)
              handleInputChange(e.target.value)
            }}
          />
        </div> */}
            <Button onClick={handleApproveArticle} className="w-full">Update</Button>
          </div>
        </TabsContent>
        <TabsContent value="guest">
          <Dialog open={isOpen} onOpenChange={setIsOpen}>
            <DialogTrigger asChild>
              <Button className="max-w-[200px] my-5 ml-auto" onClick={() => setIsOpen(true)}>Add New Guest Account</Button>
            </DialogTrigger>
            <DialogContent className=' sm:max-w-md'>
              {addNewGuestMutation.isLoading && !addNewGuestMutation.isError && <div className="absolute inset-0 z-30 flex items-center justify-center bg-white bg-opacity-50">
                <Spinner></Spinner>
              </div>}
              <DialogHeader>
                <DialogTitle>Add new guest</DialogTitle>
              </DialogHeader>
              <div className='flex items-center space-x-2'>
                <form onSubmit={handleSubmit(onAddNewGuest)} className='w-full'>
                  <div className='my-4'>
                    <Label className='text-md'>Email</Label>
                    <Controller
                      control={control}
                      name='email'
                      render={({ field }) => (
                        <Input
                          className='p-4 mt-2 outline-none'
                          placeholder='Student Email'
                          {...field}
                        ></Input>
                      )}
                    />
                    <div className='h-5 mt-3 text-base font-semibold text-red-500'>
                      {errors && errors?.email?.message}
                    </div>
                  </div>
                  <div className='my-4'>
                    <Label className='text-md'>Username</Label>
                    <div className='relative'>
                      <Controller
                        control={control}
                        name='username'
                        render={({ field }) => (
                          <Input
                            className='p-4 mt-2 outline-none'
                            placeholder='Username'
                            type={isOpen ? 'text' : 'password'}
                            {...field}
                          ></Input>
                        )}
                      />

                    </div>
                    <div className='h-5 mt-3 text-base font-semibold text-red-500'>
                      {errors && errors?.username?.message}
                    </div>
                  </div>
                  <Button
                    type='submit'
                    className={`w-full py-6 mt-8 text-lg transition-all duration-300 ease-in-out bg-blue-600 hover:bg-blue-700 ${isLoading ? 'pointer-events-none bg-opacity-65' : ''}`}
                  >
                    {isLoading ? (
                      <Spinner className={'border-white'}></Spinner>
                    ) : (
                      'Add new'
                    )}
                  </Button>
                </form>
              </div>
            </DialogContent>
          </Dialog>
        </TabsContent>
      </Tabs>


    </AdminLayout>
  )
}
