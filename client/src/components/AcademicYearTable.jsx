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
      accessorKey: 'academicYear',
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
      accessorKey: 'closureDate',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Closure Date
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
  const data = [
    {
      id: '728ed52f',
      academicYear: '2023-2024',
      closureDate: '22/03/2023',
      finalClosureDate: '05/04/2023'
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
