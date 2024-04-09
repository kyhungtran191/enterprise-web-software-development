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
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
export function EditFacultyDialog({ isOpen, handleOpenChange, data }) {
  const queryClient = useQueryClient()
  const { isLoading, mutate } = useMutation({
    mutationFn: (data) => Faculties.updateFaculty(data)
  })

  const formSchema = z.object({
    facultyName: z.string(),
    icon: z.string().url().or(z.literal(''))
  })

  const form = useForm({
    mode: 'all',
    reValidateMode: 'onChange',
    resolver: zodResolver(formSchema),
    defaultValues: {
      facultyName: data.name,
      icon: data.icon
    }
  })
  function onSubmit(facultyData) {
    if (!Object.keys(form.formState.errors).length > 0) {
      const payload = {
        ...facultyData,
        newFacultyName: facultyData.facultyName,
        facultyId: data.id
      }
      mutate(payload, {
        onSuccess: () => {
          queryClient.invalidateQueries(['adminFaculties'])
          form.reset()
          toast.success('Faculty updated successfully!')
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
    <Dialog open={isOpen} onOpenChange={handleOpenChange}>
      <DialogContent className='sm:max-w-md'>
        <DialogHeader>
          <DialogTitle>Edit faculty detail of {data.facultyName}</DialogTitle>
        </DialogHeader>
        <div className='flex items-center space-x-2'>
          <Form {...form}>
            <form
              onSubmit={form.handleSubmit(onSubmit)}
              className='space-y-4 w-full'
            >
              <FormField
                control={form.control}
                name='facultyName'
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Faculty Name</FormLabel>
                    <FormControl>
                      <Input
                        defaultValue={data.name}
                        placeholder='Faculty Name'
                        {...field}
                      />
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
                      <Input
                        defaultValue={data.facultyName}
                        placeholder='Link icon'
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <DialogFooter>
                <DialogClose
                  disabled={Object.keys(form.formState.errors).length > 0}
                >
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
        </div>
      </DialogContent>
    </Dialog>
  )
}
