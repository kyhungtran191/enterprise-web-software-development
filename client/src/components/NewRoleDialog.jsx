import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import { NewRoleForm } from './NewRoleForm'
export function NewRoleDialog({ role, permissions }) {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button>Add New</Button>
      </DialogTrigger>
      <DialogContent className='sm:max-w-md'>
        <DialogHeader>
          <DialogTitle>Add new role</DialogTitle>
        </DialogHeader>
        <div className='flex items-center space-x-2'>
          <NewRoleForm />
        </div>
      </DialogContent>
    </Dialog>
  )
}
