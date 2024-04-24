import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogClose,
  DialogDescription
} from '@/components/ui/dialog'
import { format } from 'date-fns'

export function ActivityLogDialog({
  isOpen,
  handleOpenChange,
  logs,
  contribution
}) {
  return (
    <Dialog open={isOpen} onOpenChange={handleOpenChange}>
      <DialogContent className='sm:max-w-md lg:max-w-lg xl:max-w-xl'>
        <DialogHeader>
          <DialogTitle>Activity Logs</DialogTitle>
          <DialogDescription>
            Activity logs of contribution <strong>{contribution?.title}</strong>{' '}
            by <strong>{contribution?.userName}.</strong>
          </DialogDescription>
        </DialogHeader>
        <div className='py-2'>
          {logs && logs.length > 0 ? (
            <ul className='divide-y divide-gray-200 overflow-y-auto max-h-96'>
              {logs.map((log, index) => (
                <li key={index} className='text-sm py-2'>
                  On{' '}
                  <strong>{format(new Date(log.dateCreated), 'PPpp')}</strong>,
                  user <strong>{log.userName}</strong> has '
                  <strong>{log.description}</strong>' action. From{' '}
                  <strong>{log.fromStatus}</strong> to{' '}
                  <strong>{log.toStatus}</strong>.
                </li>
              ))}
            </ul>
          ) : (
            <div className='text-center'>
              <span>No logs available.</span>
            </div>
          )}
        </div>
        <DialogFooter>
          <DialogClose asChild>
            <Button>Close</Button>
          </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
