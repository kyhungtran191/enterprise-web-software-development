import { Contributions } from "@/services/client";
import { useQuery } from "@tanstack/react-query";

export const useQueryContributionDetail = (id) => useQuery({ queryKey: ['detail-contributions'], queryFn: (_) => Contributions.getDetailPublicContribution(id) })