import { Contributions } from "@/services/client";
import { useMutation } from '@tanstack/react-query'
export const useLikeMutation = (id) => useMutation({
  mutationFn: (_) => Contributions.likeContribution(id)
})