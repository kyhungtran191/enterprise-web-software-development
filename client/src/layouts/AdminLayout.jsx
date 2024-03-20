import React from 'react'
import Header from '@/layouts/partials/Header.jsx'
import Footer from '@/layouts/partials/Footer.jsx'
import { Sidebar } from '@/layouts/partials/Sidebar.jsx'
import { Separator } from '@/components/ui/separator'
import {
  CircleGauge,
  BookText,
  UserCog,
  Settings,
  CalendarDays,
  Heart,
  User,
  MoreHorizontal,
  ArrowUpDown,
  Clock
} from 'lucide-react'

import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import { CustomTable } from '@/components/CustomTable.jsx'
import { AuthorizeDialog } from '@/components/AuthorizeDialog'

const AdminLayout = ({ children, isAdmin = true }) => {
  // TODO: This links array should be defined by fetching user roles
  // Mock Admin Sidebar options

  const links = isAdmin
    ? [
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
    : [
        {
          title: 'Recents Posts',
          icon: CircleGauge,
          href: '/student-manage/recent'
        },
        {
          title: 'Profile',
          icon: UserCog,
          href: '/student-manage/profile'
        },
        {
          title: 'Read Later',
          icon: Clock,
          href: '/student-manage/read-later'
        },
        {
          title: 'Favorites',
          icon: Heart,
          href: '/student-manage/academic-years'
        },
        {
          title: 'Settings',
          icon: Settings,
          href: '/student-manage/settings'
        }
      ]

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
            <ArrowUpDown className='w-4 h-4 ml-2' />
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
            <ArrowUpDown className='w-4 h-4 ml-2' />
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
    <>
      <Header />
      <div className='flex flex-col h-full min-h-screen border-t'>
        <div className='bg-background'>
          <div className='flex flex-row'>
            <Sidebar
              links={links}
              className={'min-w-1/4 w-1/5 hidden lg:block'}
            />
            <Separator orientation='vertical' />
            {/* Replace with correct tablle */}
            {/* <div className='w-full p-4'>
              <div className='flex flex-row justify-between'>
                <DynamicBreadcrumb />
                <Button>Add new</Button>
              </div>
              <div className='h-full px-4 py-6 lg:px-8'>
                <CustomTable columns={columns} data={data} />
              </div>
            </div> */}
            {children}
          </div>
        </div>
      </div>
      <Footer />
    </>
  )
}

export default AdminLayout
