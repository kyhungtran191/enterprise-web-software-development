import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { DialogClose } from '@radix-ui/react-dialog'
import { User } from 'lucide-react'
export function UserDialog({ isOpen, handleOpenChange, user }) {
  console.log(user)
  return (
    <Dialog open={isOpen} onOpenChange={handleOpenChange}>
      <DialogContent className='sm:max-w-[425px]'>
        <DialogHeader>
          <DialogTitle>User Information</DialogTitle>
        </DialogHeader>
        <div className='flex flex-col gap-3 py-4'>
          <div className='flex flex-col gap-4'>
            <Label htmlFor='name' className='text-left font-semibold'>
              Display Name
            </Label>
            <span>{user.displayName}</span>
          </div>
          <div className='flex flex-col gap-3'>
            <Label htmlFor='username' className='text-left font-semibold'>
              Gender
            </Label>
            <span>{user.gender}</span>
          </div>
          <div className='flex flex-col gap-3'>
            <Label htmlFor='username' className='text-left font-semibold'>
              Date of Birth
            </Label>
            <span>{user.dob}</span>
          </div>
          <div className='flex flex-col gap-3'>
            <Label htmlFor='username' className='text-left font-semibold'>
              Email
            </Label>
            <span>{user.email}</span>
          </div>
          <div className='flex flex-col gap-3'>
            <Label htmlFor='username' className='text-left font-semibold'>
              Username
            </Label>
            <span>{user.username}</span>
          </div>
          <div className='flex flex-col gap-3'>
            <Label htmlFor='username' className='text-left font-semibold'>
              User Type
            </Label>
            <span>{user.type}</span>
          </div>
          <div className='flex flex-col gap-3'>
            <Label htmlFor='username' className='text-left font-semibold'>
              Faculty
            </Label>
            <span>{user.faculty}</span>
          </div>
          <div className='flex flex-col gap-3'>
            <Label htmlFor='username' className='text-left font-semibold'>
              Status
            </Label>
            <span>{user.status}</span>
          </div>
        </div>
        <DialogFooter>
          <DialogClose>
            <Button>Close</Button>
          </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
