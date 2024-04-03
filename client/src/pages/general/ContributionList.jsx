import Article from '@/components/article'
import GeneralLayout from '@/layouts'
import React, { useCallback, useEffect, useMemo, useState } from 'react'
import {
  DropdownMenu,
  DropdownMenuContent,
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
import { isUndefined, omitBy, omit, debounce } from 'lodash'
import { useAcademicYear } from '@/query/useAcademicYear'
import { Icon } from '@iconify/react'
import Spinner from '@/components/Spinner'
import PaginationCustom from '@/components/PaginationCustom'
export default function ContributionList() {
  // State
  const [faculty, setFaculty] = useState("")
  const [academic, setAcademic] = useState("")
  const [inputValue, setInputValue] = useState('');
  //QueryData
  const { data: falcultiesData } = useFaculty()
  //
  const { data: academicData } = useAcademicYear()


  const queryParams = useParamsVariables()
  const queryConfig = omitBy(
    {
      pageindex: queryParams.pageindex || '1',
      facultyname: queryParams.facultyname,
      status: queryParams.status,
      keyword: queryParams.keyword,
      name: queryParams.name,
      year: queryParams.year,
      pagesize: queryParams.pagesize || '4',
    },
    isUndefined
  )
  const { data, isLoading } = useQuery({
    queryKey: ['contributions', queryConfig], queryFn: (_) => Contributions.getAllPublicContribution(queryConfig),
    keepPreviousData: true, staleTime: 1000
  })
  const navigate = useNavigate()

  useEffect(() => {
    if (queryParams["facultyname"]) {
      setFaculty(queryParams["facultyname"])
    }
    if (queryParams["year"]) {
      setAcademic(queryParams["year"])
    }

  }, [queryParams])


  const handleQueryByFaculty = (value) => {
    navigate({
      pathname: "/contributions",
      search: createSearchParams(omitBy({
        ...queryConfig,
        facultyname: value
      }, isUndefined)).toString()
    })
  }


  const handleQueryByAcademic = (value) => {
    navigate({
      pathname: "/contributions",
      search: createSearchParams(omitBy({
        ...queryConfig,
        year: value
      }, isUndefined)).toString()
    })
  }

  const handleInputChange = debounce((value) => {
    if (!value) {
      return navigate({
        pathname: "/contributions",
        search: createSearchParams(omit({ ...queryConfig }, ['keyword'])).toString()
      });
    }

    navigate({
      pathname: "/contributions",
      search: createSearchParams(omitBy({
        ...queryConfig,
        keyword: value
      }, (value, key) => key === 'pageindex' || key === 'pagesize' || isUndefined(value))).toString()
    });
  }, 300);

  const listData = data && data?.data?.responseData?.results
  const listFaculties = falcultiesData && falcultiesData?.data?.responseData?.results
  const listAcademic = academicData && academicData?.data?.responseData?.results

  return (
    <GeneralLayout>
      <div className="container">
        <DynamicBreadcrumb></DynamicBreadcrumb>
        <div className="flex flex-wrap items-center justify-between md:gap-5">
          <div className={`flex items-center w-full px-5 py-4 border rounded-lg gap-x-2 w-1/2`}>
            <Icon icon="ic:outline-search" className="flex-shrink-0 w-6 h-6 text-slate-700"></Icon>
            <input type="text" className='flex-1 border-none outline-none' placeholder="What you're looking for ?"
              // onChange={handleChange}
              // onKeyPress={handleKeyPress} 
              defaultValue={queryParams["keyword"]}
              onChange={(e) => {
                setInputValue(e.target.value)
                handleInputChange(e.target.value);
              }}
            />
          </div>
          <div className="flex items-center gap-5 py-5 ">
            <DropdownMenu >
              <DropdownMenuTrigger asChild disabled={isLoading}>
                <Button variant="default" className="min-w-[200px] outline-none shadow-inner text-lg font-bold p-6">{faculty || "Filter Faculty"}
                  <ArrowDown></ArrowDown>
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent className="w-56">
                <DropdownMenuSeparator />
                <DropdownMenuRadioGroup value={faculty} onValueChange={setFaculty}>
                  {!faculty == "" && <DropdownMenuRadioItem value={"All Faculty"} className={`${faculty === faculty.name ? "bg-red-500" : ""}`} key={faculty?.id} onClick={() => handleQueryByFaculty(undefined)}>All</DropdownMenuRadioItem>}
                  {listFaculties && listFaculties?.length > 0 && listFaculties?.map((faculty) => (
                    <DropdownMenuRadioItem value={faculty.name} key={faculty?.id} onClick={() => handleQueryByFaculty(faculty.name)}>{faculty.name}</DropdownMenuRadioItem>
                  ))}
                </DropdownMenuRadioGroup>
              </DropdownMenuContent>
            </DropdownMenu>
            <DropdownMenu>
              <DropdownMenuTrigger asChild disabled={isLoading}>
                <Button variant="default" className="min-w-[200px] outline-none shadow-inner text-lg font-bold p-6">{academic || "Filter Academic"}
                  <ArrowDown></ArrowDown>
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent className="w-56">
                <DropdownMenuSeparator />
                <DropdownMenuRadioGroup value={academic} onValueChange={setAcademic}>
                  {!academic == "" && <DropdownMenuRadioItem value={"All Year"} onClick={() => handleQueryByAcademic(undefined)}>All</DropdownMenuRadioItem>}
                  {listAcademic && listAcademic?.length > 0 && listAcademic?.map((academicItem) => (
                    <DropdownMenuRadioItem value={academicItem.name} key={faculty?.id} onClick={() => handleQueryByAcademic(academicItem.name)}>{academicItem.name}</DropdownMenuRadioItem>
                  ))}
                </DropdownMenuRadioGroup>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>
        </div>
        {listData && listData.length > 0 && <>
          <div className="grid gap-3 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 xl:gap-4">
            {listData.map((article) => (
              <Article article={article} key={article.id}></Article>
            ))}
          </div>
          <PaginationCustom path={"/contributions"} queryConfig={queryConfig} totalPage={data?.data?.responseData.pageCount || 1}></PaginationCustom>
        </>}
        {isLoading && <div className="flex justify-center min-h-screen mt-10">
          <Spinner></Spinner>
        </div>}
        {!listData?.length > 0 && <div className="my-10 text-3xl font-semibold text-center ">No Data</div>}
      </div>
    </GeneralLayout>
  )
}
