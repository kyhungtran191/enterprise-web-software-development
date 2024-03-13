import React from 'react'
import Header from './partials/Header'
import Footer from './partials/Footer'

export default function GeneralLayout({ children }) {
  return (
    <>
      <Header></Header>
      <div className='mt-[72px]'>{children}</div>
      <Footer></Footer>
    </>
  )
}
