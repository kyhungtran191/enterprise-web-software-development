import { AcademicYears } from '@/services/admin'
import { useQuery } from '@tanstack/react-query'
import { format } from 'date-fns'

export const useAcademicYearAdmin = (queryConfig) => {
  const { data, isLoading } = useQuery({
    queryKey: ['adminAcademicYears', queryConfig],
    queryFn: (_) => AcademicYears.getAllAcademicYears(queryConfig),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const academicYearsData = data
    ? data?.data?.responseData?.results.map((year) => ({
        ...year,
        startClosureDate: format(new Date(year.startClosureDate), 'MM-dd-yyyy'),
        endClosureDate: format(new Date(year.endClosureDate), 'MM-dd-yyyy'),
        finalClosureDate: format(new Date(year.finalClosureDate), 'MM-dd-yyyy'),
        status: year.isActive ? 'Active' : 'Inactive'
      }))
    : []
  const pageCount = data?.data?.responseData.pageCount || 1
  return { academicYearsData, pageCount, isLoading }
}
