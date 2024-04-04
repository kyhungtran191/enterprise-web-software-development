import { Contributions } from "@/services/coodinator"
import { useMutation } from "@tanstack/react-query"

export const useMutateApprove = () => useMutation({
  mutationFn: (body) => Contributions.MCApprove(body)
})