import React from 'react'
import Header from './partials/Header'
import Footer from './partials/Footer'
import { Outlet } from 'react-router-dom'
import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'

export default function GeneralLayout({ children }) {
  return (
    <>
      <Header></Header>
      <div className='flex flex-col min-h-screen mt-5'>
        {children}</div>
      <Footer></Footer>
    </>
  )
}
