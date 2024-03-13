import { cn } from '@/lib/utils'
import { Button } from '@/components/ui/button'

export function Sidebar({ className, links }) {
  return (
    <div className={cn('pb-12', className)}>
      <div className='space-y-4 py-4'>
        <div className='px-3 py-2'>
          <div className='space-y-1'>
            {links.length > 0 &&
              links.map((link, index) => (
                <>
                  <Button
                    variant={index === 0 ? 'secondary' : 'ghost'}
                    className='w-full justify-start'
                  >
                    <link.icon className='mr-2 h-4 w-4' />
                    {link.title}
                  </Button>
                </>
              ))}
          </div>
        </div>
      </div>
    </div>
  )
}
