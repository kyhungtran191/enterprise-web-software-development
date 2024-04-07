import React from 'react'
import { Avatar } from './ui/avatar'
import { AvatarFallback, AvatarImage } from '@radix-ui/react-avatar'
import { formatDate } from '@/utils/helper'

export default function Comment({ comment }) {
  return (
    <div className="my-2">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-2">
          <Avatar>
            <AvatarImage
              src={comment?.avatar}
              className='object-cover'
            ></AvatarImage>
            <AvatarFallback>CN</AvatarFallback>
          </Avatar>
          <div className="flex items-center font-semibold">{comment?.userName}</div>
        </div>
        <div className="text-sm font-medium text-gray-400">{formatDate(comment?.dateCreated)}</div>
      </div>
      <div className='p-4 font-medium'>{comment?.content}</div>
    </div>
  )
}
