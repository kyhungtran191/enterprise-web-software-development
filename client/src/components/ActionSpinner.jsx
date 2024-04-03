import React from 'react'
import Spinner from './Spinner'

export default function ActionSpinner() {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center w-screen h-full h-screen bg-black/40">
      <Spinner className="w-20 h-20 border-white"></Spinner>
    </div>
  )
}
