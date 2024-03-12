import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover'
import { Switch } from '@/components/ui/switch'
import { Icon } from '@iconify/react'
import React from 'react'
import { Link } from 'react-router-dom'

export default function Header() {
  return (
    <header className='h-[72px] w-full fixed top-0 left-0 right-0 shadow-md z-30 bg-white text-black '>
      <nav className='container h-full flex justify-between items-center leading-[72px] relative'>
        <div className='flex items-center'>
          <Link to="/" className="flex items-center justify-center gap-4">
            <img src="./logo.png" alt="logo" className='flex-shrink-0 object-cover w-10 h-10 sm:h-16 sm:w-16' />
            <h1 className='text-lg font-bold text-center sm:text-xl'>Magazine University System</h1>
          </Link>
        </div>
        <div className='items-center hidden ml-8 lg:flex absolute left-[50%] -transition-x-1/2'>
          {Array(5).fill(0).map((item, index) => (<div className="mx-4" key={index}>{index}</div>)
          )}
        </div>
        <div className='flex items-center gap-x-5'>
        <Switch />
          <Popover>
            <PopoverTrigger asChild className="cursor-pointer">
              <Avatar>
                <AvatarImage src="https://variety.com/wp-content/uploads/2021/04/Avatar.jpg" className="object-cover"></AvatarImage>
                <AvatarFallback>CN</AvatarFallback>
              </Avatar>
            </PopoverTrigger>
            <PopoverContent className="px-2 w-50">
              <div className="grid gap-4">
                <div className="space-y-2">
                  <h4 className="font-medium leading-none">trankyhung225@gmail.com</h4>
                  <p className="text-sm text-muted-foreground">
                    Student
                  </p>
                </div>
                <div className="gap-2 text-slate-700">
                  <div className="flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3"><Icon icon="mage:user-fill"></Icon>My Profile</div>
                  <div className="flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3"><Icon icon="iconoir:post-solid"></Icon>Contribution</div>
                  <div className="flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3"><Icon icon="icon-park-solid:like"></Icon>Liked Contribution</div>
                  <div className="flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3"><Icon icon="solar:logout-2-bold"></Icon>Logout</div>
                </div>
              </div>
            </PopoverContent>
          </Popover>
        </div>
      </nav>
    </header>
  )
}
