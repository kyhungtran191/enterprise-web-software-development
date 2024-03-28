import React from 'react'
import { cn } from '@/lib/utils'
import { Button } from '@/components/ui/button'
import { Link, useLocation } from 'react-router-dom'

export function Sidebar({ className, links }) {
  // Use useLocation hook to get the current location
  const location = useLocation()

  return (
    <div className={cn('pb-12', className)}>
      <div className='py-4 space-y-4'>
        <div className='px-3 py-2'>
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
                    className='justify-start w-full'
                  >
                    <link.icon className='w-4 h-4 mr-2' />
                    {link.title}
                  </Button>
                </Link>
              ))}
          </div>
        </div>
      </div>
    </div>
  )
}
