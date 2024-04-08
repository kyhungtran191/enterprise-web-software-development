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
import { clearLS } from '@/utils/auth'
import { Icon } from '@iconify/react'
import React, { useContext } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { toast } from 'react-toastify'

export default function Header() {
  const { isAuthenticated, profile, setProfile, setIsAuthenticated } = useAppContext()
  const ability = useContext(AbilityContext)
  let navigate = useNavigate()
  const handleLogout = () => {
    clearLS()
    toast.success("Logout successfully!")
    setProfile({})
    setIsAuthenticated(false)
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
            <h1 className='text-sm font-bold text-center sm:text-xl'>
              Magazine University System
            </h1>
          </Link>
        </div>
        <div className='items-center hidden ml-8 lg:flex absolute left-[50%] -transition-x-1/2'>

          <Link className='mx-4 font-semibold hover:text-blue-500' to="/contributions">
            Contribution Lists
          </Link>
          <Can I={PERMISSIONS.View.Dashboard.index} a={PERMISSIONS.View.Dashboard.value}>
            <button>Add</button>
          </Can>

        </div>
        <div className='flex items-center gap-x-5'>
          <Switch />
          <Popover>
            <PopoverTrigger asChild className='cursor-pointer'>
              <Avatar>
                <AvatarImage
                  src='https://variety.com/wp-content/uploads/2021/04/Avatar.jpg'
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
                  <p className='text-sm text-muted-foreground'>{profile && profile?.roles}({(profile?.facultyName)})</p>
                </div>
                <div className='gap-2 text-slate-700'>
                  {profile && profile.roles === Roles.Student && <>
                    <Link to="/student-manage/recent" className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3'>
                      <Icon icon='mage:user-fill'></Icon>Recent Post
                    </Link>
                    <div className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3'>
                      <Icon icon='icon-park-solid:like'></Icon>Liked Contribution
                    </div>
                  </>}

                  {profile && profile.roles === Roles.Admin && <>
                    <Link to="/admin/roles" className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3'>
                      <Icon icon='mage:user-fill'></Icon>Admin
                    </Link>


                  </>}
                  {profile && profile.roles === Roles.Coordinator &&
                    <Link to="/coodinator-manage/contributions?status=PENDING" className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3'>
                      <Icon icon='mage:user-fill'></Icon>Coodinator Manage
                    </Link>
                  }

                  <div className='flex items-center w-full px-3 py-2 rounded-lg cursor-pointer hover:bg-slate-100 gap-x-3' onClick={handleLogout}>
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
