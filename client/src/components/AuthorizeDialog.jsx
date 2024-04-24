import { useState } from 'react'
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
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { Roles } from '@/services/admin'
import Spinner from './Spinner'
import { useEffect } from 'react'
import { toast } from 'react-toastify'
export function AuthorizeDialog({ role }) {
  const queryClient = useQueryClient()
  const [permissions, setPermissions] = useState([])
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const { data, isLoading } = useQuery({
    queryKey: ['adminPermissions', role],
    queryFn: () => Roles.getPermissionsForRole(role.id),
    enabled: !!role,
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const updatePermissionsMutation = useMutation(Roles.updateRolePermissions, {
    onSuccess: () => {
      queryClient.invalidateQueries(['adminPermissions', role])
    },
    onError: () => {}
  })
  useEffect(() => {
    if (isDialogOpen && data?.data?.responseData?.roleClaims) {
      setPermissions(data.data.responseData.roleClaims)
    }
  }, [data, isDialogOpen])

  const handleCheckboxChange = (changedPermission) => {
    setPermissions((currentPermissions) =>
      currentPermissions.map((permission) =>
        permission.value === changedPermission.value
          ? { ...permission, selected: !permission.selected }
          : permission
      )
    )
  }

  const handleUpdatePermissions = () => {
    const updatedPermissions = permissions.map((permission) => ({
      type: permission.type,
      value: permission.value,
      displayName: permission.displayName,
      selected: permission.selected
    }))
    updatePermissionsMutation.mutate(
      {
        roleId: role.id,
        roleClaims: updatedPermissions
      },
      {
        onSuccess: () => {
          queryClient.invalidateQueries(['adminPermissions', role])
          toast.success('Updated permisisons successfully!')
        },
        onError: (error) => {
          const errorMessage = error?.response?.data?.title
          toast.error(errorMessage)
        }
      }
    )
  }
  return (
    <Dialog onOpenChange={setIsDialogOpen}>
      <DialogTrigger asChild>
        <Button>Authorize</Button>
      </DialogTrigger>
      <DialogContent className='sm:max-w-md overflow-y-scroll h-full'>
        <DialogHeader>
          <DialogTitle>{role.displayName} permissions</DialogTitle>
        </DialogHeader>
        {isLoading && (
          <div className='container flex items-center justify-center min-h-screen'>
            <Spinner className={'border-blue-500'}></Spinner>
          </div>
        )}

        {!isLoading &&
          permissions.map((permission) => (
            <div className='flex items-center space-x-2' key={permission.value}>
              <Checkbox
                id={permission.value}
                checked={permission.selected}
                onCheckedChange={() => handleCheckboxChange(permission)}
              />
              <Label className='font-normal'>{permission.displayName}</Label>
            </div>
          ))}

        <DialogFooter className='sm:justify-end'>
          <DialogClose>
            <Button type='button' variant='ghost'>
              Close
            </Button>
            <Button type='button' onClick={handleUpdatePermissions}>
              Update
            </Button>
          </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
