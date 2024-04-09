import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { cn } from '@/lib/utils'
import { CalendarIcon } from 'lucide-react'
import { format } from 'date-fns'
import { Button } from '@/components/ui/button'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '@/components/ui/form'
import { DialogClose, DialogFooter } from '@/components/ui/dialog'
import {
  Popover,
  PopoverContent,
  PopoverTrigger
} from '@/components/ui/popover'
import { Calendar } from '@/components/ui/calendar'
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue
} from '@/components/ui/select'
import { Input } from '@/components/ui/input'
import { useState } from 'react'
import { useQueryClient, useMutation, useQuery } from '@tanstack/react-query'
import { Faculties, Roles, Users } from '@/services/admin'
import { toast } from 'react-toastify'
import useParamsVariables from '@/hooks/useParams'
import DatePickerCustom from './DatePickerCustom'

export function NewUserForm() {
  const [userType, setUserType] = useState('')
  const queryClient = useQueryClient()
  const { isLoading, mutate } = useMutation({
    mutationFn: (data) => Users.createUser(data)
  })
  const queryParams = useParamsVariables()
  const { data: facultiesData } = useQuery({
    queryKey: ['adminFaculties', queryParams],
    queryFn: (_) => Faculties.getAllFaculties(queryParams),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const { data: rolesData } = useQuery({
    queryKey: ['adminRoles', queryParams],
    queryFn: (_) => Roles.getAllRoles(queryParams),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const faculties = facultiesData
    ? facultiesData?.data?.responseData.results
    : []
  const roles = rolesData ? rolesData?.data?.responseData : []
  const formSchema = z.object({
    firstName: z.string().min(2, {
      message: 'First name must be at least 2 characters.'
    }),
    lastName: z.string().min(2, {
      message: 'Last name must be at least 2 characters.'
    }),
    phoneNumber: z.string().min(10, {
      message: 'Phone number must be at least 10 characters.'
    }),
    username: z.string().min(2, {
      message: 'Username must be at least 6 characters.'
    }),
    email: z.string().email(),
    dob: z.date({
      message: 'A date of birth is required.'
    }),
    roleId: z.enum(
      roles.map((role) => role.id),
      {
        message: 'User type must be chosen'
      }
    ),
    facultyId: z.enum(
      faculties.map((faculty) => faculty.id),
      {
        message: 'Faculty must be chosen'
      }
    )
  })
  const form = useForm({
    mode: 'all',
    reValidateMode: 'onChange',
    resolver: zodResolver(formSchema),
    defaultValues: {
      firstName: '',
      lastName: '',
      phoneNumber: '',
      username: '',
      role: '',
      email: '',
      dob: '',
      faculty: ''
    }
  })
  async function getImageAsBinaryString() {
    const response = await fetch('/client/public/avatar.png')
    if (!response.ok) {
      throw new Error('Failed to fetch default avatar')
    }
    const blob = await response.blob()
    return new Promise((resolve, reject) => {
      const reader = new FileReader()
      reader.onloadend = () => resolve(reader.result)
      reader.onerror = reject
      reader.readAsBinaryString(blob)
    })
  }
  async function onSubmit(formData) {
    try {
      const avatarBinaryString = await getImageAsBinaryString()
      const payload = {
        ...formData,
        avatar: btoa(avatarBinaryString),
        isActive: true
      }
      mutate(payload, {
        onSuccess: () => {
          queryClient.invalidateQueries(['adminUsers'])
          form.reset()
          toast.success('User created successfully!')
        },
        onError: (error) => {
          const errorMessage = error?.response?.data?.title
          toast.error(errorMessage)
          setIsSubmitting(false)
        }
      })
      console.log(payload)
    } catch (error) {
      console.log(error)
    }
  }
  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className='space-y-4 w-full'>
        <FormField
          control={form.control}
          name='firstName'
          render={({ field }) => (
            <FormItem>
              <FormLabel>First Name</FormLabel>
              <FormControl>
                <Input placeholder='First Name' {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name='lastName'
          render={({ field }) => (
            <FormItem>
              <FormLabel>Last Name</FormLabel>
              <FormControl>
                <Input placeholder='Last Name' {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name='username'
          render={({ field }) => (
            <FormItem>
              <FormLabel>Username</FormLabel>
              <FormControl>
                <Input placeholder='Username' {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name='password'
          render={({ field }) => (
            <FormItem>
              <FormLabel>Password</FormLabel>
              <FormControl>
                <Input
                  type='password'
                  placeholder='Enter the password'
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name='email'
          render={({ field }) => (
            <FormItem>
              <FormLabel>Email</FormLabel>
              <FormControl>
                <Input placeholder='Email' {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name='phoneNumber'
          render={({ field }) => (
            <FormItem>
              <FormLabel>Phone number</FormLabel>
              <FormControl>
                <Input type='tel' placeholder='Phone number' {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name='dob'
          render={({ field }) => (
            <FormItem className='flex flex-col'>
              <FormLabel>Date of birth</FormLabel>
              <Popover>
                <PopoverTrigger asChild>
                  <FormControl>
                    <Button
                      variant={'outline'}
                      className={cn(
                        'w-full w-min-[240px] pl-3 text-left font-normal',
                        !field.value && 'text-muted-foreground'
                      )}
                    >
                      {field.value ? (
                        format(field.value, 'PPP')
                      ) : (
                        <span>Pick a date</span>
                      )}
                      <CalendarIcon className='ml-auto h-4 w-4 opacity-50' />
                    </Button>
                  </FormControl>
                </PopoverTrigger>
                <PopoverContent className='w-auto p-0' align='start'>
                  <DatePickerCustom
                    mode='single'
                    captionLayout='dropdown-buttons'
                    selected={field.value}
                    onSelect={(date) => {
                      field.onChange(date)
                    }}
                    disabled={(date) => date < new Date('2000-01-01')}
                    initialFocus
                    fromYear={1960}
                    toYear={new Date().getFullYear()}
                  />
                </PopoverContent>
              </Popover>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name='roleId'
          render={({ field }) => (
            <FormItem>
              <FormLabel>User Roles</FormLabel>
              <FormControl>
                <Select onValueChange={field.onChange}>
                  <SelectTrigger className='w-min-[180px] w-full'>
                    <SelectValue placeholder='Select role' />
                  </SelectTrigger>
                  <SelectContent>
                    {roles?.map((role) => (
                      <SelectItem key={role.id} value={role.id}>
                        {role.displayName}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        {userType !== 'marketingManager' && (
          <FormField
            control={form.control}
            name='facultyId'
            render={({ field }) => (
              <FormItem>
                <FormLabel>Faculty</FormLabel>
                <FormControl>
                  <Select onValueChange={field.onChange}>
                    <SelectTrigger className='w-min-[180px] w-full'>
                      <SelectValue placeholder='Select faculty' />
                    </SelectTrigger>
                    <SelectContent>
                      {faculties?.map((faculty) => (
                        <SelectItem key={faculty.id} value={faculty.id}>
                          {faculty.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        )}
        <DialogFooter>
          <DialogClose disabled={Object.keys(form.formState.errors).length > 0}>
            <Button
              type='submit'
              disabled={Object.keys(form.formState.errors).length > 0}
            >
              Submit
            </Button>
          </DialogClose>
        </DialogFooter>
      </form>
    </Form>
  )
}
