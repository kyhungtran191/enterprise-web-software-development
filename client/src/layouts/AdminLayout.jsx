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
const AdminLayout = () => {
  const links = [
    {
      title: 'Dashboard',
      icon: CircleGauge
    },
    {
      title: 'Contributions',
      icon: BookText
    },
    {
      title: 'Academic Years',
      icon: CalendarDays
    },
    {
      title: 'Users',
      icon: User
    },
    {
      title: 'Roles & Permissions',
      icon: UserCog
    },
    {
      title: 'Settings',
      icon: Settings
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
            <div className=''>
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
