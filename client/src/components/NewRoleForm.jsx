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
import { useQueryClient, useMutation } from '@tanstack/react-query'
import { Input } from '@/components/ui/input'
import { toast } from 'react-toastify'
import { Roles } from '@/services/admin'
const formSchema = z.object({
  name: z.string({ required_error: 'A display name is required.' }),
  displayName: z.string({ required_error: 'A display name is required.' })
})

export function NewRoleForm() {
  const queryClient = useQueryClient()
  const { isLoading, mutate } = useMutation({
    mutationFn: (data) => Roles.createRole(data)
  })
  const form = useForm({
    resolver: zodResolver(formSchema),
    defaultValues: {
      name: '',
      displayName: ''
    }
  })

  function onSubmit(values) {
    if (!Object.keys(form.formState.errors).length > 0) {
      mutate(values, {
        onSuccess: () => {
          queryClient.invalidateQueries(['adminRoles'])
          form.reset()
          toast.success('Academic year created successfully!')
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
          name='displayName'
          render={({ field }) => (
            <FormItem>
              <FormLabel>Display Name</FormLabel>
              <FormControl>
                <Input placeholder='Display Name' {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name='name'
          render={({ field }) => (
            <FormItem>
              <FormLabel>Name</FormLabel>
              <FormControl>
                <Input placeholder='Name' {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <DialogFooter>
          <DialogClose>
            <Button type='submit'>Submit</Button>
          </DialogClose>
        </DialogFooter>
      </form>
    </Form>
  )
}
