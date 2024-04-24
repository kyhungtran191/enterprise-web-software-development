import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { cn } from '@/lib/utils'
import { CalendarIcon } from 'lucide-react'
import { format } from 'date-fns'
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
import { useState } from 'react'
import { useEffect } from 'react'
import { useQueryClient, useMutation } from '@tanstack/react-query'
import { AcademicYears } from '@/services/admin'
import { toast } from 'react-toastify'
import DatePickerCustom from './DatePickerCustom'
export function AcademicYearEditDialog({ isOpen, handleOpenChange, data }) {
  const [academicYears, setAcademicYears] = useState([])
  const [academicYear, setAcademicYear] = useState(data.name)
  const [startClosureDate, setStartClosureDate] = useState(
    data ? new Date(data.startClosureDate) : null
  )
  const [endClosureDate, setEndClosureDate] = useState(
    data ? new Date(data.endClosureDate) : null
  )
  const [finalClosureDate, setFinalClosureDate] = useState(
    data ? new Date(data.finalClosureDate) : null
  )
  const queryClient = useQueryClient()
  const { isLoading, mutate } = useMutation({
    mutationFn: (data) => AcademicYears.updateAcademicYear(data)
  })

  function isDateWithinAcademicYear(date, academicYear) {
    const [startYear, endYear] = academicYear.split('-')
    const academicYearStart = new Date(`${startYear}-01-01`)
    const academicYearEnd = new Date(`${endYear}-12-31`)
    return date >= academicYearStart && date <= academicYearEnd
  }

  const formSchema = z.object({
    academicYearName: z.string(),
    startClosureDate: z.date().refine(
      (startDate) => {
        return isDateWithinAcademicYear(startDate, academicYear)
      },
      {
        message:
          'Start closure date must be within the selected academic year and before the end closure date.'
      }
    ),
    endClosureDate: z.date().refine(
      (endDate) => {
        return (
          endDate > startClosureDate &&
          isDateWithinAcademicYear(endDate, academicYear)
        )
      },
      {
        message:
          'End closure date must be within the selected academic year and before the final closure date.'
      }
    ),
    finalClosureDate: z.date().refine(
      (finalDate) => {
        return (
          finalDate > endClosureDate &&
          isDateWithinAcademicYear(finalDate, academicYear)
        )
      },
      {
        message:
          'Final closure date must be within the selected academic year and after end closure date.'
      }
    )
  })

  useEffect(() => {
    const currentYear = new Date().getFullYear()
    const years = []
    for (let i = 0; i < 10; i++) {
      const startYear = currentYear + i
      const endYear = startYear + 1
      years.push(`${startYear}-${endYear}`)
    }
    setAcademicYears(years)
  }, [])
  const form = useForm({
    mode: 'all',
    reValidateMode: 'onChange',
    resolver: zodResolver(formSchema),
    defaultValues: {
      academicYearName: academicYear,
      startClosureDate: startClosureDate,
      endClosureDate: endClosureDate,
      finalClosureDate: finalClosureDate
    }
  })
  console.log(form.getValues())
  function onSubmit(academicYearData) {
    console.log(academicYearData)
    academicYearData.academicYearId = data.id
    if (!Object.keys(form.formState.errors).length > 0) {
      mutate(academicYearData, {
        onSuccess: () => {
          queryClient.invalidateQueries(['adminAcademicYears'])
          form.reset()
          setStartClosureDate(null)
          setEndClosureDate(null)
          setFinalClosureDate(null)
          toast.success('Academic year edited successfully!')
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
  console.log(academicYears)
  console.log('data', data)
  return (
    <Dialog open={isOpen} onOpenChange={handleOpenChange}>
      <DialogContent className='sm:max-w-md'>
        <DialogHeader>
          <DialogTitle>Edit academic year detail of {data.name}</DialogTitle>
        </DialogHeader>
        <div className='flex items-center space-x-2'>
          <Form {...form}>
            <form
              onSubmit={form.handleSubmit(onSubmit)}
              className='space-y-4 w-full'
            >
              <FormField
                control={form.control}
                name='academicYearName'
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Select Academic Year</FormLabel>
                    <FormControl>
                      <Select
                        onValueChange={(value) => {
                          setAcademicYear(value)
                          field.onChange(value)
                        }}
                        defaultValue={data.name}
                      >
                        <SelectTrigger className='w-min-[180px] w-full'>
                          <SelectValue placeholder='Select academic year' />
                        </SelectTrigger>
                        <SelectContent>
                          {academicYears.map((year) => (
                            <SelectItem key={year} value={year}>
                              {year}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name='startClosureDate'
                render={({ field }) => (
                  <FormItem className='flex flex-col'>
                    <FormLabel>Start Closure Date</FormLabel>
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
                            setStartClosureDate(date)
                          }}
                          disabled={(date) => date < new Date('2000-01-01')}
                          initialFocus
                          fromYear={new Date().getFullYear()}
                          toYear={new Date().getFullYear() + 10}
                        />
                      </PopoverContent>
                    </Popover>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name='endClosureDate'
                render={({ field }) => (
                  <FormItem className='flex flex-col'>
                    <FormLabel>End Closure Date</FormLabel>
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
                            setEndClosureDate(date)
                          }}
                          disabled={(date) => date < new Date('2000-01-01')}
                          initialFocus
                          fromYear={new Date().getFullYear()}
                          toYear={new Date().getFullYear() + 10}
                        />
                      </PopoverContent>
                    </Popover>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name='finalClosureDate'
                render={({ field }) => (
                  <FormItem className='flex flex-col'>
                    <FormLabel>Final Closure Date</FormLabel>
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
                            setFinalClosureDate(date)
                          }}
                          disabled={(date) => date < new Date('2000-01-01')}
                          initialFocus
                          fromYear={new Date().getFullYear()}
                          toYear={new Date().getFullYear() + 10}
                        />
                      </PopoverContent>
                    </Popover>
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
