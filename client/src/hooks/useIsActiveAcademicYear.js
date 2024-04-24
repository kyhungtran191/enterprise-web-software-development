import { useAcademicYear } from '@/query/useAcademicYear'

export default function GetCurrentAcademicYear() {
  const { data } = useAcademicYear()
  const detailData = data?.data?.responseData?.results.filter(
    (item) => item?.isActive
  )
  return detailData && detailData[0]
}

export function IsOutDeadlineAdd() {
  const data = GetCurrentAcademicYear()
  if (!data) return true
  const now = new Date()
  const specifiedTime = new Date(data?.endClosureDate)

  return now > specifiedTime ? true : false
}

export function IsOutDeadlineUpdate() {
  const data = GetCurrentAcademicYear()
  if (!data) return true
  const now = new Date()
  const specifiedTime = new Date(data?.endFinalDate)
  return now > specifiedTime ? true : false
}
