import React from 'react'

export default function Loading({ background = "bg-white" }) {
  return (
    <div className="flex flex-row gap-2">
      <div className={`w-4 h-4 rounded-full ${background} animate-bounce [animation-delay:-.5s]`} />
      <div className={`w-4 h-4 rounded-full ${background} animate-bounce [animation-delay:-.3s]`} />
      <div className={`w-4 h-4 rounded-full ${background} animate-bounce [animation-delay:-.1s]`} />
    </div>

  )
}
