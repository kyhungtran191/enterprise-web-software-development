import { Contributions } from "@/services/client";
import { useQuery } from "@tanstack/react-query";

export const useFaculty = () => useQuery({
  queryKey: ['faculty'], queryFn: Contributions.getAllFaculties,
})
