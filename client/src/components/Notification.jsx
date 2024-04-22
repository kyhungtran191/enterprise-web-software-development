import React from 'react'
import { Popover, PopoverContent, PopoverTrigger } from './ui/popover'
import { Bell } from 'lucide-react'
import { Link } from 'react-router-dom'
import { Avatar, AvatarFallback, AvatarImage } from './ui/avatar'
import { Separator } from '@radix-ui/react-dropdown-menu'

export default function Notification() {
  return (
    <div className='relative flex items-center justify-center w-12 h-12 transition-colors duration-300 ease-in-out rounded-full cursor-pointer hover:bg-slate-100'>
      <div className="absolute w-[10px] h-[10px] bg-red-500 rounded-full bottom-6 left-6 z-20"></div>
      <Popover className="">
        <PopoverTrigger className='flex items-center justify-center w-12 h-12'>
          <Bell className='relative w-6 h-6'>
          </Bell>
        </PopoverTrigger>
        <PopoverContent className="p-0 h-[350px] overflow-scroll-y">
          <div className="">
            {/* Notificate component */}
            <div>
              <Link className='flex items-center w-full gap-5 px-3 py-2 hover:bg-slate-100'>
                <Avatar>
                  <AvatarImage
                    src={`https://variety.com/wp-content/uploads/2021/04/Avatar.jpg`}
                    className='object-cover'
                  ></AvatarImage>
                  <AvatarFallback>CN</AvatarFallback>
                </Avatar>
                <div className="flex-1">
                  <span className='text-xs font-bold line-clamp-3'>Tran Ky Hung sent you a message Tran Ky Hung sent you a message
                    Tran Ky Hung sent you a message  Tran Ky Hung sent you a message Tran Ky Hung sent you a message Tran Ky Hung sent you a message
                    Tran Ky Hung sent you a message  Tran Ky Hung sent you a message</span>
                </div>
              </Link>
              <Separator className="my-2" />
            </div>
          </div>
        </PopoverContent>
      </Popover>
    </div>
  )
}
