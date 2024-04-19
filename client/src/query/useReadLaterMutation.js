import { Contributions } from "@/services/client";
import { useMutation } from '@tanstack/react-query'
export const useReadLaterMutation = (id) => useMutation({
  mutationFn: (_) => Contributions.addReadLater(id)
})