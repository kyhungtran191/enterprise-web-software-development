import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import { NewFacultyForm } from './NewFacultyForm'
export function NewFacultyDialog() {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button>Add New</Button>
      </DialogTrigger>
      <DialogContent className='sm:max-w-md'>
        <DialogHeader>
          <DialogTitle>Add new faculty</DialogTitle>
        </DialogHeader>
        <div className='flex items-center space-x-2'>
          <NewFacultyForm />
        </div>
      </DialogContent>
    </Dialog>
  )
}
