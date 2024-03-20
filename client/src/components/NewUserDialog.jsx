import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import { NewUserForm } from './NewUserForm'
export function NewUserDialog({ role, permissions }) {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button>Add New</Button>
      </DialogTrigger>
      <DialogContent className='sm:max-w-md'>
        <DialogHeader>
          <DialogTitle>Add new user</DialogTitle>
        </DialogHeader>
        <div className='flex items-center space-x-2'>
          <NewUserForm />
        </div>
      </DialogContent>
    </Dialog>
  )
}
