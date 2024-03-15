import React from 'react'
import Header from '@/layouts/partials/Header.jsx'
import Footer from '@/layouts/partials/Footer.jsx'
import { Sidebar } from '@/layouts/partials/Sidebar.jsx'
import { Separator } from '@/components/ui/separator'
import {
  CircleGauge,
  User,
  BookText,
  UserCog,
  Settings,
  CalendarDays,
  MoreHorizontal,
  ArrowUpDown
} from 'lucide-react'

import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Button } from '@/components/ui/button'
import { DataTable } from '@/components/Table'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'
const AdminLayout = () => {
  // TODO: This links array should be defined by fetching user roles
  const links = [
    {
      title: 'Dashboard',
      icon: CircleGauge,
      href: '/admin/dashboard'
    },
    {
      title: 'Contributions',
      icon: BookText,
      href: '/admin/contributions'
    },
    {
      title: 'Academic Years',
      icon: CalendarDays,
      href: '/admin/academic-years'
    },
    {
      title: 'Users',
      icon: User,
      href: '/admin/users'
    },
    {
      title: 'Roles & Permissions',
      icon: UserCog,
      href: '/admin/roles'
    },
    {
      title: 'Settings',
      icon: Settings,
      href: '/admin/settings'
    }
  ]
  const columns = [
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
      accessorKey: 'amount',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Amount
            <ArrowUpDown className='ml-2 h-4 w-4' />
          </Button>
        )
      }
    },
    {
      id: 'actions',
      cell: ({ row }) => {
        const payment = row.original
        return (
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant='ghost' className='h-8 w-8 p-0'>
                <span className='sr-only'>Open menu</span>
                <MoreHorizontal className='h-4 w-4' />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align='end'>
              <DropdownMenuLabel>Actions</DropdownMenuLabel>
              <DropdownMenuItem
                onClick={() => navigator.clipboard.writeText(payment.id)}
              >
                Copy payment ID
              </DropdownMenuItem>
              <DropdownMenuSeparator />
              <DropdownMenuItem>View customer</DropdownMenuItem>
              <DropdownMenuItem>View payment details</DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        )
      }
    }
  ]
  const data = [
    {
      id: '728ed52f',
      amount: 100,
      status: 'pending',
      email: 'm@example.com'
    },
    {
      id: '489e1d42',
      amount: 125,
      status: 'processing',
      email: 'example@gmail.com'
    }
  ]
  return (
    <>
      <Header />
      <div className='border-t h-full'>
        <div className='bg-background'>
          <div className='flex flex-row'>
            <Sidebar
              links={links}
              className={'min-w-1/4 w-1/5 hidden lg:block'}
            />
            <Separator orientation='vertical' />
            <div className='p-4 w-full'>
              <div className='flex flex-row justify-between'>
                <DynamicBreadcrumb />
                <Button>Add new</Button>
              </div>
              <div className='h-full px-4 py-6 lg:px-8'>
                <DataTable columns={columns} data={data} />
              </div>
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </>
  )
}

export default AdminLayout
