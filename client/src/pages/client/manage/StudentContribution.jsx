import Search from '@/components/Search'
import { Button } from '@/components/ui/button'
import AdminLayout from '@/layouts/AdminLayout'
import { ArrowDown10Icon, Plus } from 'lucide-react'
import React, { useEffect, useState } from 'react'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuRadioGroup,
  DropdownMenuRadioItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'

import Article from '@/components/article'
import { createSearchParams, useNavigate } from 'react-router-dom'
import { QueryClient, useQuery } from '@tanstack/react-query'
import useParamsVariables from '@/hooks/useParams'
import { Contributions } from '@/services/client'
import { isUndefined, omitBy, omit, debounce } from 'lodash'
import { Icon } from '@iconify/react'
import PaginationCustom from '@/components/PaginationCustom'
import Spinner from '@/components/Spinner'
import { STUDENT_OPTIONS } from '@/constant/menuSidebar'
export default function StudentContribution() {
  const [position, setPosition] = React.useState('')
  const navigate = useNavigate()
  const queryParams = useParamsVariables()
  const [inputValue, setInputValue] = useState('');
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
    queryKey: ['recent', queryConfig],
    queryFn: (_) => Contributions.getCurrentContribution(queryConfig)
  })

  useEffect(() => {
    if (position != "") {
      navigate({
        pathname: "/manage/recent",
        search: createSearchParams(omitBy({
          ...queryConfig,
          status: position
        }, (value, key) => key === 'pageindex' || key === 'pagesize' || isUndefined(value))).toString()
      });
    } else {
      return navigate({
        pathname: "/manage/recent",
        search: createSearchParams(omit({ ...queryConfig }, ['status'])).toString()
      });
    }

  }, [position])

  useEffect(() => {
    if (queryParams["status"]) {
      setPosition(queryParams["status"])
    }
  }, queryParams)

  const handleInputChange = debounce((value) => {
    if (!value) {
      return navigate({
        pathname: "/manage/recent",
        search: createSearchParams(omit({ ...queryConfig }, ['keyword'])).toString()
      });
    }

    navigate({
      pathname: "/manage/recent",
      search: createSearchParams(omitBy({
        ...queryConfig,
        keyword: value
      }, (value, key) => key === 'pageindex' || key === 'pagesize' || isUndefined(value))).toString()
    });
  }, 300);

  const currentData = data && data?.data?.responseData;
  return (
    <AdminLayout links={STUDENT_OPTIONS}>
      <div className='flex flex-wrap items-center gap-3 my-5'>
        <div className={`flex items-center px-5 py-4 border rounded-lg gap-x-2 w-[50vw]`}>
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
        <div className='flex flex-wrap items-center gap-2'>
          <Button className='flex-1 py-7' onClick={() => navigate("/manage/add-contribution")}>
            <Plus></Plus>
            Add new Article
          </Button>
          <div className='flex-1'>
            <DropdownMenu>
              <DropdownMenuTrigger asChild className='w-full'>
                <Button
                  variant='default'
                  className='gap-4 border-none outline-none py-7 min-w-[145px]'
                >
                  {position.toUpperCase() || "Filter Status"} <ArrowDown10Icon></ArrowDown10Icon>
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent className='w-56'>
                <DropdownMenuRadioGroup
                  value={position}
                  onValueChange={setPosition}
                >
                  {position != "" && <DropdownMenuRadioItem value=''>
                    All
                  </DropdownMenuRadioItem>}
                  <DropdownMenuRadioItem value='PENDING'>
                    Pending
                  </DropdownMenuRadioItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuRadioItem value='APPROVE'>
                    Approve
                  </DropdownMenuRadioItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuRadioItem value='REJECT'>
                    Reject
                  </DropdownMenuRadioItem>
                </DropdownMenuRadioGroup>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>
        </div>
      </div>
      {currentData && currentData?.results?.length > 0 && <>
        {currentData && currentData?.results
          ?.map((article, index) => (
            <Article key={index} isRevert={true} status={article?.status} className={'my-4'}
              article={article}></Article>
          ))}
        <PaginationCustom path={"/manage/recent"} queryConfig={queryConfig} totalPage={data?.data?.responseData.pageCount || 1}></PaginationCustom>
      </>}
      {isLoading && <div className="flex justify-center min-h-screen mt-10">
        <Spinner></Spinner>
      </div>}
      {!isLoading && !currentData?.results?.length > 0 && <div className="my-10 text-3xl font-semibold text-center ">No Data</div>}
    </AdminLayout>
  )
}
