import { Contributions } from "@/services/client";
import { useQuery } from "@tanstack/react-query";

export const useAcademicYear = () => useQuery({
  queryKey: ['academicClient'], queryFn: Contributions.getAllAcademicYear,
})
