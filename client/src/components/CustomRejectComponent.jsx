import React, { useState } from 'react'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Label } from "@/components/ui/label"
import { Button } from './ui/button'
import * as yup from "yup"
import { yupResolver } from "@hookform/resolvers/yup"
import { useForm } from 'react-hook-form'
import { useMutation, useQueryClient } from "@tanstack/react-query"
import { Contributions } from '@/services/coodinator'
import Spinner from './Spinner'
import { toast } from 'react-toastify'
import { Link, useNavigate } from 'react-router-dom'
export default function CustomRejectComponent({ id }) {
  const [open, setOpen] = useState(false)
  const navigate = useNavigate()
  const schema = yup
    .object({
      note: yup.string().required("Please provide reason"),
    })


  const { mutate, isLoading } = useMutation({
    mutationFn: (data) => Contributions.MCReject(data)
  })
  const { register, handleSubmit, formState: { errors }, setError, reset } = useForm({
    resolver: yupResolver(schema)
  })
  const onSubmit = (data) => {
    mutate({ id, note: data.note }, {
      onSuccess() {
        toast.success("Rejected this contribution successfully")
        navigate('/coodinator-manage/contributions?status=REJECT')
      },
      onError(error) {
        console.log(error)
      }
    })
  }
  return (
    <Dialog defaultOpen={false} onOpenChange={setOpen} open={open} onClick={e => { e.stopPropagation() }}>
      <DialogTrigger>
        <Button className="bg-red-500 hover:bg-red-600 ">Reject</Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        {isLoading && <div className="fixed inset-0 flex items-center justify-center bg-black/30">
          <Spinner className={"border-white"}></Spinner>
        </div>}
        <DialogHeader>
          <DialogTitle>Reject Student Contribtuion</DialogTitle>
          <DialogDescription>
            Make sure you have the right decision to your student contributions.
          </DialogDescription>
        </DialogHeader>
        <form className="grid gap-4 py-4" onSubmit={handleSubmit(onSubmit)}>
          <Label htmlFor="username" className="text-left">
            Reason
          </Label>
          <textarea
            id=""
            defaultValue="I need more reasons to accept this contribution, please provide more detail in your contribution"
            className="w-full min-h-[300px] outline-none border-2 p-2"
            {...register("note")}
          />
          <div className="my-3 font-semibold text-red-500">{errors && errors?.note?.message}</div>
          <Button type="submit" className="w-full bg-red-500 hover:bg-red-600">Complete Reject</Button>
        </form>

      </DialogContent>
    </Dialog>
  )
}
