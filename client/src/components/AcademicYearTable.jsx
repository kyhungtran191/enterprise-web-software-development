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
import { useQuery } from '@tanstack/react-query'
import useParamsVariables from '@/hooks/useParams'
import { AcademicYears } from '@/services/admin'
import Spinner from './Spinner'
export function AcademicYearTable() {
  const [isOpenViewUser, setIsOpenViewUser] = useState(false)
  const [viewUser, setViewUser] = useState({})
  const handleViewUser = (user) => {
    setIsOpenViewUser(true)
    setViewUser(user)
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
                <DropdownMenuItem onSelect={() => handleViewUser(row.original)}>
                  <User className='w-4 h-4 mr-2' />
                  <span>View detail</span>
                </DropdownMenuItem>
                <DropdownMenuItem>
                  <Pencil className='w-4 h-4 mr-2' />
                  <span>Edit academic year</span>
                </DropdownMenuItem>
                <DropdownMenuItem>
                  <UserRoundX className='w-4 h-4 mr-2' />
                  <span>Activate year</span>
                </DropdownMenuItem>
                <DropdownMenuItem>
                  <UserRoundX className='w-4 h-4 mr-2' />
                  <span>Deactivate year</span>
                </DropdownMenuItem>
              </DropdownMenuGroup>
            </DropdownMenuContent>
          </DropdownMenu>
        )
      }
    }
  ]
  const queryParams = useParamsVariables()
  const { data, isLoading } = useQuery({
    queryKey: ['academicYears', queryParams],
    queryFn: (_) => AcademicYears.getAllAcademicYears(queryParams),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const academicYearsData =
    data &&
    data?.data?.responseData?.results.map((year) => ({
      ...year,
      startClosureDate: format(new Date(year.startClosureDate), 'dd-MM-yyyy'),
      endClosureDate: format(new Date(year.endClosureDate), 'dd-MM-yyyy'),
      finalClosureDate: format(new Date(year.finalClosureDate), 'dd-MM-yyyy')
    }))
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
          <CustomTable columns={columns} data={academicYearsData} />
        </div>
      )}

      {/* <UserDialog
        isOpen={isOpenViewUser}
        handleOpenChange={setIsOpenViewUser}
        user={viewUser}
      /> */}
    </div>
  )
}
