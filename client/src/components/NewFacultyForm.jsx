import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { z } from 'zod'
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
import { Input } from '@/components/ui/input'
import { useQueryClient, useMutation } from '@tanstack/react-query'
import { Faculties } from '@/services/admin'
import { toast } from 'react-toastify'
export function NewFacultyForm() {
  const queryClient = useQueryClient()
  const { isLoading, mutate } = useMutation({
    mutationFn: (data) => Faculties.createFaculty(data)
  })

  const formSchema = z.object({
    facultyName: z.string(),
    icon: z.string().url()
  })

  const form = useForm({
    mode: 'all',
    reValidateMode: 'onChange',
    resolver: zodResolver(formSchema),
    defaultValues: {
      facultyName: '',
      icon: ''
    }
  })

  function onSubmit(facultyData) {
    if (!Object.keys(form.formState.errors).length > 0) {
      mutate(facultyData, {
        onSuccess: () => {
          queryClient.invalidateQueries(['adminFaculties'])
          form.reset()
          toast.success('Faculty created successfully!')
        },
        onError: (error) => {
          const errorMessage = error?.response?.data?.title
          toast.error(errorMessage)
        }
      })
    } else {
      toast.error('Please fill in all required fields correctly.')
    }
  }
  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className='space-y-4 w-full'>
        <FormField
          control={form.control}
          name='facultyName'
          render={({ field }) => (
            <FormItem>
              <FormLabel>Faculty Name</FormLabel>
              <FormControl>
                <Input placeholder='Faculty Name' {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name='icon'
          render={({ field }) => (
            <FormItem>
              <FormLabel>Faculty Icon</FormLabel>
              <FormControl>
                <Input placeholder='Link icon' {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
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
