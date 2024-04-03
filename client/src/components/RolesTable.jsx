import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import { CustomTable } from '@/components/CustomTable.jsx'
import { AuthorizeDialog } from '@/components/AuthorizeDialog'
import { ArrowUpDown } from 'lucide-react'
import DynamicBreadcrumb from './DynamicBreadcrumbs'
import { NewRoleDialog } from './NewRoleDialog'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import useParamsVariables from '@/hooks/useParams'
import { Roles } from '@/services/admin'
import Spinner from './Spinner'
export function RolesTable() {
  const queryClient = useQueryClient()
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
            Name
            <ArrowUpDown className='ml-2 h-4 w-4' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'displayName',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Display Name
            <ArrowUpDown className='ml-2 h-4 w-4' />
          </Button>
        )
      }
    },
    {
      id: 'actions',
      cell: ({ row }) => {
        return <AuthorizeDialog role={row.original} />
      }
    }
  ]
  const queryParams = useParamsVariables()
  let { data, isLoadingRoles } = useQuery({
    queryKey: ['adminRoles', queryParams],
    queryFn: (_) => Roles.getAllRoles(queryParams),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const rolesData = data ? data?.data?.responseData : []
  console.log(rolesData)
  return (
    <div className='w-full p-4'>
      <div className='flex flex-row justify-between'>
        <DynamicBreadcrumb />
        <NewRoleDialog />
      </div>
      {isLoadingRoles && (
        <div className='container flex items-center justify-center min-h-screen'>
          <Spinner className={'border-blue-500'}></Spinner>
        </div>
      )}
      {!isLoadingRoles && (
        <div className='h-full px-4 py-6 lg:px-8'>
          <CustomTable columns={columns} data={rolesData} />
        </div>
      )}
    </div>
  )
}
