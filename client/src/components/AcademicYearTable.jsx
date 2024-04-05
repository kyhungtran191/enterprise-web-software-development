import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import { CustomTable } from '@/components/CustomTable.jsx'
import { ArrowUpDown } from 'lucide-react'
import DynamicBreadcrumb from './DynamicBreadcrumbs'
import { format } from 'date-fns'
import { Pencil, UserRoundX, EllipsisVertical, User } from 'lucide-react'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'
import { useState } from 'react'
import { NewAcademicYearDialog } from './NewAcademicYearDialog'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import useParamsVariables from '@/hooks/useParams'
import { AcademicYears } from '@/services/admin'
import { toast } from 'react-toastify'
import Spinner from './Spinner'
import { AcademicYearEditDialog } from './AcademicYearEditDialog'
import { isUndefined, omitBy } from 'lodash'
export function AcademicYearTable() {
  const [isOpenEditAcademicYear, setIsOpenEditAcademicYear] = useState(false)
  const [academicYear, setAcademicYear] = useState({})
  const queryClient = useQueryClient()
  const { mutate: activateAcademicYear } = useMutation({
    mutationFn: (id) => AcademicYears.activateAcademicYear(id)
  })
  const { mutate: deactivateAcademicYear } = useMutation({
    mutationFn: (id) => AcademicYears.deactivateAcademicYear(id)
  })

  const handleEditAcademicYear = (academicYear) => {
    setIsOpenEditAcademicYear(true)
    setAcademicYear(academicYear)
  }
  const handleActivateAcademicYear = (academicYear) => {
    activateAcademicYear(academicYear.id, {
      onSuccess: () => {
        queryClient.invalidateQueries('adminAcademicYears')
        toast.success('Activated successfully!')
      },
      onError: (error) => {
        const errorMessage = error?.response?.data?.title
        toast.error(errorMessage)
      }
    })
  }
  const handleDeactivateAcademicYear = (academicYear) => {
    deactivateAcademicYear(academicYear.id, {
      onSuccess: () => {
        queryClient.invalidateQueries('adminAcademicYears')
        toast.success('Deactivated successfully!')
      },
      onError: (error) => {
        const errorMessage = error?.response?.data?.title
        toast.error(errorMessage)
      }
    })
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
          onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
          aria-label='Select all'
          className='mx-4'
        />
      ),
      cell: ({ row }) => (
        <Checkbox
          checked={row.getIsSelected()}
          onCheckedChange={(value) => row.toggleSelected(!!value)}
          aria-label='Select row'
        />
      ),
      enableSorting: false,
      enableHiding: false
    },
    {
      accessorKey: 'name',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Academic Year
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'startClosureDate',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Start Closure Date
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'endClosureDate',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            End Closure Date
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'finalClosureDate',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Final Closure Date
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'status',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Status
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      id: 'actions',
      cell: ({ row }) => {
        return (
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <EllipsisVertical className='cursor-pointer' />
            </DropdownMenuTrigger>
            <DropdownMenuContent className='w-56'>
              <DropdownMenuGroup>
                <DropdownMenuItem
                  onSelect={() => handleEditAcademicYear(row.original)}
                >
                  <Pencil className='w-4 h-4 mr-2' />
                  <span>Edit academic year</span>
                </DropdownMenuItem>
                <DropdownMenuItem
                  onSelect={() => handleActivateAcademicYear(row.original)}
                >
                  <UserRoundX className='w-4 h-4 mr-2' />
                  <span>Activate this year</span>
                </DropdownMenuItem>
                <DropdownMenuItem
                  onSelect={() => handleDeactivateAcademicYear(row.original)}
                >
                  <UserRoundX className='w-4 h-4 mr-2' />
                  <span>Deactivate this year</span>
                </DropdownMenuItem>
              </DropdownMenuGroup>
            </DropdownMenuContent>
          </DropdownMenu>
        )
      }
    }
  ]
  const queryParams = useParamsVariables()
  const queryConfig = omitBy(
    {
      pageindex: queryParams.pageindex || '1',
      pagesize: queryParams.pagesize || '5'
    },
    isUndefined
  )
  const { data, isLoading } = useQuery({
    queryKey: ['adminAcademicYears', queryConfig],
    queryFn: (_) => AcademicYears.getAllAcademicYears(queryConfig),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const academicYearsData = data
    ? data?.data?.responseData?.results.map((year) => ({
        ...year,
        startClosureDate: format(new Date(year.startClosureDate), 'MM-dd-yyyy'),
        endClosureDate: format(new Date(year.endClosureDate), 'MM-dd-yyyy'),
        finalClosureDate: format(new Date(year.finalClosureDate), 'MM-dd-yyyy'),
        status: year.isActive ? 'Active' : 'Inactive'
      }))
    : []
  return (
    <div className='w-full p-4'>
      <div className='flex flex-row justify-between'>
        <DynamicBreadcrumb />
        <NewAcademicYearDialog />
      </div>
      {isLoading && (
        <div className='container flex items-center justify-center min-h-screen'>
          <Spinner className={'border-blue-500'}></Spinner>
        </div>
      )}
      {!isLoading && (
        <div className='h-full px-4 py-6 lg:px-8'>
          <CustomTable
            columns={columns}
            data={academicYearsData}
            path={'/admin/academic-years'}
            queryConfig={queryConfig}
            pageCount={data?.data?.responseData.pageCount || 1}
          />
        </div>
      )}
      {Object.keys(academicYear).length > 0 && (
        <AcademicYearEditDialog
          isOpen={isOpenEditAcademicYear}
          handleOpenChange={setIsOpenEditAcademicYear}
          data={academicYear}
        />
      )}
    </div>
  )
}
