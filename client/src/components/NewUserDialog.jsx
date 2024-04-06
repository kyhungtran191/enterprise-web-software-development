import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import { NewUserForm } from './NewUserForm'
import { useState } from 'react'
export function NewUserDialog() {
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isOpen, setIsOpen] = useState(false)
  const closeDialog = () => setIsOpen(false)

  return (
    <Dialog open={isOpen} onOpenChange={setIsOpen}>
      <DialogTrigger asChild>
        <Button onClick={() => setIsOpen(true)}>Add New</Button>
      </DialogTrigger>
      <DialogContent className='sm:max-w-md'>
        <DialogHeader>
          <DialogTitle>Add new user</DialogTitle>
        </DialogHeader>
        <div className='flex items-center space-x-2'>
          <NewUserForm
            isSubmitting={isSubmitting}
            setIsSubmitting={setIsSubmitting}
            closeDialog={closeDialog}
          />
        </div>
      </DialogContent>
    </Dialog>
  )
}
