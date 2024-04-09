import { Roles } from "@/constant/roles";
import { useAppContext } from "@/hooks/useAppContext";
import { Contributions } from "@/services/client";
import { useQuery } from "@tanstack/react-query";

export const useFaculty = () => {
  const { profile } = useAppContext()
  return useQuery({
    queryKey: ['faculty'], queryFn: Contributions.getAllFaculties,
  })
}