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
  User
} from 'lucide-react'

import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Button } from '@/components/ui/button'

const AdminLayout = ({ children }) => {
  // TODO: This links array should be defined by fetching user roles
  // Mock Admin Sidebar options
  // const links = [
  //   {
  //     title: 'Dashboard',
  //     icon: CircleGauge,
  //     href: '/admin/dashboard'
  //   },
  //   {
  //     title: 'Contributions',
  //     icon: BookText,
  //     href: '/admin/contributions'
  //   },
  //   {
  //     title: 'Academic Years',
  //     icon: CalendarDays,
  //     href: '/admin/academic-years'
  //   },
  //   {
  //     title: 'Users',
  //     icon: User,
  //     href: '/admin/users'
  //   },
  //   {
  //     title: 'Roles & Permissions',
  //     icon: UserCog,
  //     href: '/admin/roles'
  //   },
  //   {
  //     title: 'Settings',
  //     icon: Settings,
  //     href: '/admin/settings'
  //   }
  // ]

  const links = [
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
            <div className='w-full p-4'>
              <div className='flex flex-row justify-between'>
                <DynamicBreadcrumb />
                <Button>Add new</Button>
              </div>
              <div className='h-full px-4 py-6 lg:px-8'>{children}</div>
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </>
  )
}

export default AdminLayout
