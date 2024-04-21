import { AbilityContext, Can } from '@/components/casl/Can'
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import {
  Popover,
  PopoverContent,
  PopoverTrigger
} from '@/components/ui/popover'
import { Switch } from '@/components/ui/switch'
import { PERMISSIONS } from '@/constant/casl-permissions'
import { Roles } from '@/constant/roles'
import { useAppContext } from '@/hooks/useAppContext'
import { clearLS, deletePermissions } from '@/utils/auth'
import { Icon } from '@iconify/react'
import React, { useContext } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'
import { useQueryClient } from '@tanstack/react-query'
import { Clock, BellRing, MessageSquare } from 'lucide-react'
export default function Header() {
  const queryClient = useQueryClient()
  const { isAuthenticated, profile, setProfile, setIsAuthenticated, avatar } =
    useAppContext()
  const ability = useContext(AbilityContext)
  let navigate = useNavigate()
  const handleLogout = () => {
    clearLS()
    setProfile({})
    setIsAuthenticated(false)
    deletePermissions()
    queryClient.clear()
    toast.success('Logout successfully!')
    navigate('/login')
  }
  return (
    <header className='h-[72px] w-full sticky top-0 left-0 right-0 shadow-md z-30 bg-white text-black'>
      <nav className='container h-full flex justify-between items-center leading-[72px] relative'>
        <div className='flex items-center'>
          <Link to='/' className='flex items-center justify-center gap-4'>
            <img
              src='../../logo.png'
              alt='logo'
              className='flex-shrink-0 object-cover w-10 h-10 sm:h-16 sm:w-16'
            />
            <h1 className='hidden text-sm font-bold text-center sm:block sm:text-xl'>
              Magazine University System
            </h1>
          </Link>
        </div>
        <div className='items-center hidden ml-8 lg:flex absolute left-[50%] -transition-x-1/2'>
          {profile?.roles !== Roles.Guest && (
            <Link
              className='mx-4 font-semibold hover:text-blue-500'
              to='/contributions'
            >
              Contribution Lists
            </Link>
          )}
          {/* <Can I={PERMISSIONS.View.Dashboard.index} a={PERMISSIONS.View.Dashboard.value}>
            <button>Add</button>
          </Can> */}
        </div>
        <div className='flex items-center gap-x-2'>
          <Link
            to='/message'
            className='flex items-center justify-center w-12 h-12 transition-colors duration-300 ease-in-out rounded-full cursor-pointer hover:bg-slate-100'
          >
            <MessageSquare></MessageSquare>
          </Link>
          <div className='flex items-center justify-center w-12 h-12 transition-colors duration-300 ease-in-out rounded-full cursor-pointer hover:bg-slate-100'>
            <BellRing></BellRing>
          </div>

          <Popover>
            <PopoverTrigger asChild className='cursor-pointer'>
              <Avatar>
                <AvatarImage
                  src={`${avatar || 'https://variety.com/wp-content/uploads/2021/04/Avatar.jpg'}`}
                  className='object-cover'
                ></AvatarImage>
                <AvatarFallback>CN</AvatarFallback>
              </Avatar>
            </PopoverTrigger>
            <PopoverContent className='px-2 w-50'>
              <div className='grid gap-4'>
                <div className='space-y-2'>
                  <h4 className='font-medium leading-none'>
                    {profile && profile?.email}
                  </h4>
                  <p className='text-sm text-muted-foreground'>
                    {profile && profile?.roles}({profile?.facultyName})
                  </p>
                </div>
                <div className='gap-2 text-slate-700'>
                  {profile && profile.roles === Roles.Student && (
                    <>
                      <Link
                        to='/student-manage/recent'
                        className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3'
                      >
                        <Clock></Clock>Recent Post
                      </Link>
                      <Link
                        to='/student-manage/favorites'
                        className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3'
                      >
                        <Icon icon='icon-park-solid:like'></Icon>Liked
                        Contribution
                      </Link>
                    </>
                  )}
                  {profile && profile.roles === Roles.Admin && (
                    <>
                      <Link
                        to='/admin/dashboard'
                        className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3'
                      >
                        <Icon icon='mage:user-fill'></Icon>Admin
                      </Link>
                    </>
                  )}
                  {profile && profile.roles === Roles.Manager && (
                    <>
                      <Link
                        to='/mm/dashboard'
                        className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3'
                      >
                        <Icon icon='mage:user-fill'></Icon>Manager
                      </Link>
                    </>
                  )}
                  {profile && profile.roles === Roles.Coordinator && (
                    <Link
                      to='/coodinator-manage/contributions?status=PENDING'
                      className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3'
                    >
                      <Icon icon='mage:user-fill'></Icon>Coodinator Manage
                    </Link>
                  )}
                  {profile && profile.roles !== Roles?.Guest && (
                    <Link
                      to='/profile'
                      className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3'
                    >
                      <Icon icon='mage:user-fill'></Icon>Profile
                    </Link>
                  )}
                  <div
                    className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3'
                    onClick={handleLogout}
                  >
                    <Icon icon='solar:logout-2-bold'></Icon>Logout
                  </div>
                </div>
              </div>
            </PopoverContent>
          </Popover>
        </div>
      </nav>
    </header>
  )
}
