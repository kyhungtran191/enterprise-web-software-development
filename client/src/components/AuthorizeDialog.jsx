import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import { Checkbox } from '@/components/ui/checkbox'
import { Label } from './ui/label'
export function AuthorizeDialog({ role, permissions }) {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button>Authorize</Button>
      </DialogTrigger>
      <DialogContent className='sm:max-w-md'>
        <DialogHeader>
          <DialogTitle>{role} permissions</DialogTitle>
        </DialogHeader>
        {permissions.map((permission) => (
          <div className='flex items-center space-x-2' key={permission.id}>
            <Checkbox id={permission} />
            <Label className='font-normal'>{permission.name}</Label>
          </div>
        ))}
        <DialogFooter className='sm:justify-end'>
          <DialogClose>
            <Button type='button' variant='ghost'>
              Close
            </Button>
          </DialogClose>
          <Button type='button'>Update</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
