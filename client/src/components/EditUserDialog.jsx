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
import { useContext, useState } from 'react'
import { useQueryClient, useMutation, useQuery } from '@tanstack/react-query'
import { Faculties, Roles, Users } from '@/services/admin'
import { toast } from 'react-toastify'
import useParamsVariables from '@/hooks/useParams'
import {
  DialogContent,
  Dialog,
  DialogHeader,
  DialogTitle
} from '@/components/ui/dialog'
import DatePickerCustom from './DatePickerCustom'
import { Roles as ROLES } from '@/constant/roles'
import { AppContext } from '@/contexts/app.context'
export function EditUserDialog({
  isOpen,
  handleOpenChange,
  data,
  isSubmitting,
  setIsSubmitting,
  closeDialog
}) {
  const [userType, setUserType] = useState(data.role)
  const queryClient = useQueryClient()
  const { isLoading, mutate } = useMutation({
    mutationFn: (data) => Users.updateUserById(data)
  })
  const { profile } = useContext(AppContext)
  const queryParams = useParamsVariables()
  const { data: facultiesData, isLoading: isFacultiesLoading } = useQuery({
    queryKey: ['adminFaculties', queryParams],
    queryFn: (_) => Faculties.getAllFacultiesPaging(queryParams),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const { data: rolesData, isLoading: isRolesLoading } = useQuery({
    queryKey: ['adminRoles', queryParams],
    queryFn: (_) => Roles.getAllRoles(),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const faculties = facultiesData
    ? facultiesData?.data?.responseData.results
    : []
  const roles = rolesData
    ? profile.roles === ROLES.Admin
      ? rolesData?.data?.responseData
      : rolesData?.data?.responseData.filter(
          (role) => role.name === ROLES.Coordinator
        )
    : []
  const formSchema = z.object({
    email: z.string().email(),
    userName: z.string().min(6),
    roleId: z.enum(
      roles.map((role) => role.id),
      {
        message: 'User type must be chosen'
      }
    ),
    facultyId:
      userType === ROLES.Admin || userType === ROLES.Manager
        ? z.string()
        : z.enum(
            faculties.map((faculty) => faculty.id),
            {
              message: 'Faculty must be chosen'
            }
          ),
    firstName: z.string(),
    lastName: z.string(),
    phoneNumber: z.string(),
    dob: z.date()
  })
  const form = useForm({
    mode: 'all',
    reValidateMode: 'onChange',
    resolver: zodResolver(formSchema),
    defaultValues: {
      firstName: data.firstName || '',
      lastName: data.lastName || '',
      phoneNumber: data.phoneNumber || '',
      userName: data.userName,
      roleId: roles?.find((role) => role.name === data.role)?.id,
      facultyId: faculties?.find((faculty) => faculty.name === data.faculty)
        ?.id,
      email: data.email,
      dob: new Date(data.dob) || null
    }
  })
  console.log(form.getValues())
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
    setIsSubmitting(true)
    try {
      const avatarBinaryString = await getImageAsBinaryString()
      const payload = {
        ...formData,
        id: data.id,
        avatar: btoa(avatarBinaryString),
        isActive: true
      }
      mutate(payload, {
        onSuccess: () => {
          queryClient.invalidateQueries({ queryKey: ['adminUsers'] })
          toast.success('User created successfully!')
          form.reset()
          setIsSubmitting(false)
          closeDialog()
        },
        onError: (error) => {
          const errorMessage = error?.response?.data?.title
          toast.error(errorMessage)
          setIsSubmitting(false)
        }
      })
    } catch (error) {
      console.log(error)
    }
  }
  return (
    <Dialog open={isOpen} onOpenChange={handleOpenChange}>
      <DialogContent className='sm:max-w-md'>
        <DialogHeader>
          <DialogTitle>Update user detail of {data.userName}</DialogTitle>
        </DialogHeader>
        <Form {...form}>
          <form
            onSubmit={form.handleSubmit(onSubmit)}
            className='w-full space-y-4'
          >
            <FormField
              control={form.control}
              name='firstName'
              render={({ field }) => (
                <FormItem>
                  <FormLabel>First Name</FormLabel>
                  <FormControl>
                    <Input
                      defaultValue={data.firstName}
                      placeholder='First Name'
                      {...field}
                    />
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
                    <Input
                      defaultValue={data.lastName}
                      placeholder='Last Name'
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name='userName'
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Username</FormLabel>
                  <FormControl>
                    <Input
                      defaultValue={data.userName}
                      placeholder='Username'
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
                    <Input
                      defaultValue={data.email}
                      placeholder='Email'
                      {...field}
                    />
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
                    <Input
                      type='tel'
                      defaultValue={data.phoneNumber}
                      placeholder='Phone number'
                      {...field}
                    />
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
                          <CalendarIcon className='w-4 h-4 ml-auto opacity-50' />
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
                    <Select
                      onValueChange={(fieldValue) => {
                        const selectedRole = roles.find(
                          (role) => role.id === fieldValue
                        )
                        if (selectedRole) {
                          field.onChange(selectedRole.id)
                          setUserType(selectedRole.name)
                        }
                      }}
                      defaultValue={form.getValues('roleId')}
                    >
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
            {userType !== ROLES.Admin && userType !== ROLES.Manager && (
              <FormField
                control={form.control}
                name='facultyId'
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Faculty</FormLabel>
                    <FormControl>
                      <Select
                        onValueChange={field.onChange}
                        defaultValue={
                          faculties?.find(
                            (faculty) => faculty.name === data.faculty
                          )?.id
                        }
                      >
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
              <Button
                type='submit'
                disabled={
                  Object.keys(form.formState.errors).length > 0 || isSubmitting
                }
              >
                Submit
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}
