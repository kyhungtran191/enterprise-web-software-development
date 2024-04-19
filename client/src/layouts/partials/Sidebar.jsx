import React from 'react'
import { cn } from '@/lib/utils'
import { Button } from '@/components/ui/button'
import { Link, useLocation } from 'react-router-dom'

export function Sidebar({ className, links }) {
  // Use useLocation hook to get the current location
  const location = useLocation()

  return (
    <>
      <div className={cn('pb-12 shadow-xl', className)}>
        <div className='py-4 space-y-4'>
          <div className='medium:py-2 medium:px-3'>
            <div className='space-y-1'>
              {links.length > 0 &&
                links.map((link, index) => (
                  <Link to={link.href} key={index}>
                    {' '}
                    <Button
                      key={index}
                      variant={
                        location.pathname === link.href ? 'secondary' : 'ghost'
                      }
                      className='justify-center medium:justify-start w-full !px-2 !medium:py-2 !medium:px-4'
                    >
                      <link.icon className='w-4 h-4 mr-2' />
                      <span className='hidden medium:block'>{link.title}</span>
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
