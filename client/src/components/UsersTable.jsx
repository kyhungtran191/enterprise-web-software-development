import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import { CustomTable } from '@/components/CustomTable.jsx'
import { AuthorizeDialog } from '@/components/AuthorizeDialog'
import { ArrowUpDown } from 'lucide-react'
import DynamicBreadcrumb from './DynamicBreadcrumbs'
export function UsersTable() {
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
      accessorKey: 'username',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Username
            <ArrowUpDown className='ml-2 h-4 w-4' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'email',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Email
            <ArrowUpDown className='ml-2 h-4 w-4' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'faculty',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Faculty
            <ArrowUpDown className='ml-2 h-4 w-4' />
          </Button>
        )
      }
    },
    {
      id: 'actions',
      cell: ({ row }) => {
        return (
          // change this to actions for users
          <AuthorizeDialog role={row.original.displayName} permissions={[]} />
        )
      }
    }
  ]
  const data = [
    {
      id: '728ed52f',
      displayName: 'Nguyen Minh Khang',
      email: 'khang@gmail.com',
      faculty: 'IT'
    },
    {
      id: '728ed52f',
      displayName: 'Tran Ky Hung',
      email: 'hung@gmail.com',
      faculty: 'IT'
    }
  ]

  return (
    <div className='w-full p-4'>
      <div className='flex flex-row justify-between'>
        <DynamicBreadcrumb />
        <Button>Add new</Button>
      </div>
      <div className='h-full px-4 py-6 lg:px-8'>
        <CustomTable columns={columns} data={data} />
      </div>
    </div>
  )
}
