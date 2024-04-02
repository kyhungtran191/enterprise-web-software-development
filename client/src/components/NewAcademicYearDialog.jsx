import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import { NewAcademicYearForm } from './NewAcademicYearForm'
export function NewAcademicYearDialog() {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button>Add New</Button>
      </DialogTrigger>
      <DialogContent className='sm:max-w-md'>
        <DialogHeader>
          <DialogTitle>Add new academic year</DialogTitle>
        </DialogHeader>
        <div className='flex items-center space-x-2'>
          <NewAcademicYearForm />
        </div>
      </DialogContent>
    </Dialog>
  )
}
