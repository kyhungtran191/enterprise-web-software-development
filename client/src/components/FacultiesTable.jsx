import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import { CustomTable } from '@/components/CustomTable.jsx'
import { ArrowUpDown } from 'lucide-react'
import DynamicBreadcrumb from './DynamicBreadcrumbs'
import { format } from 'date-fns'
import { Pencil, UserRoundX, EllipsisVertical } from 'lucide-react'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'
import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import useParamsVariables from '@/hooks/useParams'
import { AcademicYears, Faculties } from '@/services/admin'
import { toast } from 'react-toastify'
import Spinner from './Spinner'
import { isUndefined, omitBy } from 'lodash'
import { NewFacultyDialog } from './NewFacultyDiaglog'
import { EditFacultyDialog } from './EditFacultyDialog'
export function FacultiesTable() {
  const [isOpenEditFaculty, setIsOpenEditFaculty] = useState(false)
  const [selectedRow, setSelectedRow] = useState({})
  const [faculty, setFaculty] = useState({})
  const queryClient = useQueryClient()
  const handleEditFaculty = (faculty) => {
    setIsOpenEditFaculty(true)
    setFaculty(faculty)
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
            Faculty Name
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'dateCreated',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Date Created
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
                  onSelect={() => handleEditFaculty(row.original)}
                >
                  <Pencil className='w-4 h-4 mr-2' />
                  <span>Edit faculty</span>
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
      pagesize: queryParams.pagesize || '10'
    },
    isUndefined
  )
  const { data, isLoading } = useQuery({
    queryKey: ['adminFaculties', queryConfig],
    queryFn: (_) => Faculties.getAllFacultiesPaging(queryConfig),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const facultiesData = data
    ? data?.data?.responseData?.results.map((faculty) => ({
        ...faculty,
        dateCreated: format(new Date(faculty.dateCreated), 'MM-dd-yyyy')
      }))
    : []
  return (
    <div className='w-full p-4'>
      <div className='flex flex-row justify-between'>
        <DynamicBreadcrumb />
        <NewFacultyDialog />
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
            data={facultiesData}
            path={'/admin/faculties'}
            queryConfig={queryConfig}
            pageCount={data?.data?.responseData.pageCount || 1}
            selectedRows={setSelectedRow}
          />
        </div>
      )}
      {Object.keys(faculty).length > 0 && (
        <EditFacultyDialog
          isOpen={isOpenEditFaculty}
          handleOpenChange={setIsOpenEditFaculty}
          data={faculty}
        />
      )}
    </div>
  )
}
