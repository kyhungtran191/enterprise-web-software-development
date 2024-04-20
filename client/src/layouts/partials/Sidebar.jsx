import React from 'react'
import { cn } from '@/lib/utils'
import { Button } from '@/components/ui/button'
import { Link, useLocation } from 'react-router-dom'
import { ShieldCloseIcon } from 'lucide-react'

export function Sidebar({ className, links }) {
  // Use useLocation hook to get the current location
  const location = useLocation()

  return (
    <>
      <div className={cn('', className)}>
        <div className='py-4 space-y-4 '>
          <div className='medium:py-2 medium:px-3 '>
            <div className='flex flex-wrap items-center gap-1 space-y-1 justify-evenly sm:justify-start sm:flex-col'>

              {links.length > 0 &&
                links.map((link, index) => (
                  <Link to={link.href} key={index} >
                    {' '}
                    <Button
                      key={index}
                      variant={
                        location.pathname === link.href ? 'secondary' : 'ghost'
                      }
                      className='justify-center medium:justify-start w-full !px-2 !medium:py-2 !medium:px-4 flex flex-col sm:flex-row'
                    >
                      <link.icon className='flex-shrink-0 w-4 h-4 mr-2' />
                      <span className='block sm:hidden medium:block'>{link.title}</span>
                    </Button>
                  </Link>
                ))}
            </div>
          </div>
        </div>
      </div>

    </>
  )
}
