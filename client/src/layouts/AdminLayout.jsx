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
  CalendarDays
} from 'lucide-react'

import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Button } from '@/components/ui/button'

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
              <div className='h-full px-4 py-6 lg:px-8'>List roles</div>
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </>
  )
}

export default AdminLayout
