import React from 'react'

export default function Spinner({ className }) {
  return (
    <div className={`w-10 h-10 border-t-4 border-b-4 border-white rounded-full animate-spin ${className}`}></div>
  )
}
