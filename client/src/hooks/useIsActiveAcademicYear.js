import { useAcademicYear } from "@/query/useAcademicYear"



export default function GetCurrentAcademicYear() {
  const { data } = useAcademicYear()
  const detailData = data?.data?.responseData?.results.filter((item) => item?.isActive)
  return detailData && detailData[0]
}

export function IsOutDeadlineAdd() {
  const data = GetCurrentAcademicYear()
  const now = new Date();
  const specifiedTime = new Date(data?.endClosureDate);
  console.log(now, specifiedTime)
  return now > specifiedTime ? true : false
}