import React from 'react'
import Header from '@/layouts/partials/Header.jsx'
import Footer from '@/layouts/partials/Footer.jsx'
import { Sidebar } from '@/layouts/partials/Sidebar.jsx'
import { Separator } from '@/components/ui/separator'

import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import { CustomTable } from '@/components/CustomTable.jsx'
import { AuthorizeDialog } from '@/components/AuthorizeDialog'
import { ADMIN_OPTIONS } from '@/constant/menuSidebar'

const AdminLayout = ({ children, isAdmin = true, links = ADMIN_OPTIONS }) => {
  // TODO: This links array should be defined by fetching user roles
  // Mock Admin Sidebar options
  return (
    <>
      <Header />
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
          <div className='flex flex-col flex-1 min-h-screen p-4 overflow-x-auto'>
            {children}
          </div>
        </div>
      </div>
      <Footer />
    </>
  )
}

export default AdminLayout
