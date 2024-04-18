import { Roles } from "@/constant/roles"
import { useAppContext } from "@/hooks/useAppContext"
import { GuestContribution } from "@/services/guest"
import { useQuery } from "@tanstack/react-query"
export const useGuestContribution = () => {
  const { profile } = useAppContext()
  return useQuery({
    queryKey: ['guest'], queryFn: GuestContribution.getContribution,
    enabled: profile && profile?.roles === Roles.Guest
  })
}