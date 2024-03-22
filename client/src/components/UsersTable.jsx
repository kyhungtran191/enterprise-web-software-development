import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import { CustomTable } from '@/components/CustomTable.jsx'
import { ArrowUpDown } from 'lucide-react'
import DynamicBreadcrumb from './DynamicBreadcrumbs'
import { NewUserDialog } from './NewUserDialog'
import { Pencil, UserRoundX, EllipsisVertical, User } from 'lucide-react'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'
import { UserDialog } from './UserDialog'
import { useState } from 'react'

export function UsersTable() {
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
      accessorKey: 'type',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            User Type
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
      accessorKey: 'status',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Status
            <ArrowUpDown className='ml-2 h-4 w-4' />
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
                  <User className='mr-2 h-4 w-4' />
                  <span>View User</span>
                </DropdownMenuItem>
                <DropdownMenuItem>
                  <Pencil className='mr-2 h-4 w-4' />
                  <span>Edit user</span>
                </DropdownMenuItem>
                <DropdownMenuItem>
                  <UserRoundX className='mr-2 h-4 w-4' />
                  <span>Deactivate user</span>
                </DropdownMenuItem>
              </DropdownMenuGroup>
            </DropdownMenuContent>
          </DropdownMenu>
        )
      }
    }
  ]
  const data = [
    {
      id: '728ed52f',
      username: 'khang1233',
      displayName: 'Nguyen Minh Khang',
      gender: 'Male',
      status: 'Active',
      type: 'Student',
      email: 'khang@gmail.com',
      dob: '2003-22-03',
      faculty: 'IT'
    },
    {
      id: '728ed52f',
      username: 'hung1233',
      displayName: 'Tran Ky Hung',
      type: 'Student',
      gender: 'Male',
      dob: '2003-22-03',
      email: 'hung@gmail.com',
      status: 'Inactive',
      faculty: 'IT'
    }
  ]

  return (
    <div className='w-full p-4'>
      <div className='flex flex-row justify-between'>
        <DynamicBreadcrumb />
        <NewUserDialog />
      </div>
      <div className='h-full px-4 py-6 lg:px-8'>
        <CustomTable columns={columns} data={data} />
      </div>
      <UserDialog
        isOpen={isOpenViewUser}
        handleOpenChange={setIsOpenViewUser}
        user={viewUser}
      />
    </div>
  )
}
