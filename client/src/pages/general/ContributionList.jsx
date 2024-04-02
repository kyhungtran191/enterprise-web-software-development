import Article from '@/components/article'
import GeneralLayout from '@/layouts'
import React, { useCallback, useEffect, useMemo, useState } from 'react'
import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination"

import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuRadioGroup,
  DropdownMenuRadioItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'

import { useQuery, useQueryClient } from '@tanstack/react-query'
import useParamsVariables from '@/hooks/useParams'
import DynamicBreadcrumb from '@/components/DynamicBreadcrumbs'
import { Contributions } from '@/services/client'

import { useFaculty } from '@/query/useFaculty'
import { Button } from '@/components/ui/button'
import { ArrowDown } from 'lucide-react'
import { createSearchParams, useNavigate } from 'react-router-dom'
export default function ContributionList() {
  const [faculty, setFaculty] = useState("")
  //QueryData
  const { data: falcultiesData } = useFaculty()
  //

  const queryParams = useParamsVariables()
  const { data } = useQuery({
    queryKey: ['contributions', queryParams], queryFn: (_) => Contributions.getAllPublicContribution(queryParams),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const navigate = useNavigate()

  useEffect(() => {
    if (queryParams["facultyName"]) {
      setFaculty(queryParams["facultyName"])
    }
  }, [queryParams])

  const handleQueryByFaculty = (value) => {

    navigate({
      pathname: "/contributions",
      search: createSearchParams({
        ...queryParams,
        facultyName: value
      }).toString()
    })
  }
  const listData = data && data?.data?.responseData?.results
  const listFaculties = falcultiesData && falcultiesData?.data?.responseData?.results
  return (
    <GeneralLayout>
      <div className="container">
        <DynamicBreadcrumb></DynamicBreadcrumb>
        <div className="flex items-center gap-5 py-10 ">
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant="default" className="min-w-[200px] outline-none shadow-inner text-lg font-bold p-6">{faculty || "Filter Faculty"}
                <ArrowDown></ArrowDown>
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent className="w-56">
              <DropdownMenuSeparator />
              <DropdownMenuRadioGroup value={faculty.name}>
                {listFaculties && listFaculties?.length > 0 && listFaculties?.map((faculty) => (
                  <DropdownMenuRadioItem value={faculty.name} key={faculty?.id} onClick={() => handleQueryByFaculty(faculty.name)}>{faculty.name}</DropdownMenuRadioItem>
                ))}
              </DropdownMenuRadioGroup>
            </DropdownMenuContent>
          </DropdownMenu>

        </div>
        <div className="grid gap-3 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 xl:gap-4">
          {listData && listData.length > 0 && listData.map((article) => (
            <Article article={article} key={article.id}></Article>
          ))}
        </div>

        <Pagination>
          <PaginationContent>
            <PaginationItem>
              <PaginationPrevious href="#" />
            </PaginationItem>
            <PaginationItem>
              <PaginationLink href="#">1</PaginationLink>
            </PaginationItem>
            <PaginationItem>
              <PaginationEllipsis />
            </PaginationItem>
            <PaginationItem>
              <PaginationNext href="#" />
            </PaginationItem>
          </PaginationContent>
        </Pagination>
      </div>
    </GeneralLayout>
  )
}
