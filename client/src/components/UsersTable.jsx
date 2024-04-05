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
import useParamsVariables from '@/hooks/useParams'
import { Users } from '@/services/admin'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import Spinner from './Spinner'
import { format } from 'date-fns'
import { isUndefined, omitBy } from 'lodash'
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
      accessorKey: 'firstName',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            First Name
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'lastName',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Last Name
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },

    {
      accessorKey: 'userName',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Username
            <ArrowUpDown className='w-4 h-4 ml-2' />
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
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'phoneNumber',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Phone Number
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'dob',

      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Date of Birth
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'role',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Role
            <ArrowUpDown className='w-4 h-4 ml-2' />
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
                  <span>View User</span>
                </DropdownMenuItem>
                <DropdownMenuItem>
                  <Pencil className='w-4 h-4 mr-2' />
                  <span>Edit user</span>
                </DropdownMenuItem>
                <DropdownMenuItem>
                  <UserRoundX className='w-4 h-4 mr-2' />
                  <span>Deactivate user</span>
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
    queryKey: ['adminUsers', queryConfig],
    queryFn: (_) => Users.getAllUsers(queryConfig),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const users = data
    ? data?.data?.responseData?.results?.map((user) => {
        return {
          ...user,
          dob: format(new Date(user.dob), 'MM-dd-yyyy'),
          status: user.isActive ? 'Active' : 'Inactive',
          role: user.roles[0]
        }
      })
    : []
  return (
    <div className='w-full p-4'>
      <div className='flex flex-row justify-between'>
        <DynamicBreadcrumb />
        <NewUserDialog />
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
            data={users}
            path={'/admin/users'}
            queryConfig={queryConfig}
            pageCount={data?.data?.responseData.pageCount || 1}
          />
        </div>
      )}
      <UserDialog
        isOpen={isOpenViewUser}
        handleOpenChange={setIsOpenViewUser}
        user={viewUser}
      />
    </div>
  )
}
