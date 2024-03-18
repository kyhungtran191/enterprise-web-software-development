import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import { CustomTable } from '@/components/CustomTable.jsx'
import { AuthorizeDialog } from '@/components/AuthorizeDialog'
import { ArrowUpDown } from 'lucide-react'
import DynamicBreadcrumb from './DynamicBreadcrumbs'
export function RolesTable() {
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
        return (
          <AuthorizeDialog
            role={row.original.displayName}
            permissions={permissionList}
          />
        )
      }
    }
  ]
  const data = [
    {
      id: '728ed52f',
      name: 'Admin',
      displayName: 'Administrator'
    },
    {
      id: '728ed52f',
      name: 'Student',
      displayName: 'Student'
    }
  ]
  const permissionList = [
    {
      id: '1',
      name: 'View contribution'
    },
    {
      id: '2',
      name: 'Create contribution'
    },
    {
      id: '3',
      name: 'Edit contribution'
    },
    {
      id: '4',
      name: 'Delete contribution'
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
