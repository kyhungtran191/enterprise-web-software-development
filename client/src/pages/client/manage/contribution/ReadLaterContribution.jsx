import Article from '@/components/article'
import GeneralLayout from '@/layouts'
import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react'
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
import AdminLayout from '@/layouts/AdminLayout'
import { STUDENT_OPTIONS } from '@/constant/menuSidebar'
export default function ReadLaterContribution() {
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
    queryKey: ['readlater-list', queryConfig], queryFn: (_) => Contributions.getReadLater(queryConfig),
    keepPreviousData: true, staleTime: 1000
  })
  const navigate = useNavigate()

  const listData = data && data?.data?.responseData
  return (
    <AdminLayout links={STUDENT_OPTIONS}>
      <div className="container py-5">
        <DynamicBreadcrumb></DynamicBreadcrumb>
        <div className="flex flex-wrap items-center justify-between md:gap-5">
        </div>
        {listData && listData.length > 0 && <>
          <div className="">
            {listData.map((article) => (
              <Article article={article} key={article.id} classImageCustom="!h-[300px]" isRevert={true}></Article>
            ))}
          </div>
          <PaginationCustom path={"/student-manage/favorites"} queryConfig={queryConfig} totalPage={data?.data?.responseData.pageCount || 1}></PaginationCustom>
        </>}
        {isLoading && <div className="flex justify-center min-h-screen mt-10">
          <Spinner></Spinner>
        </div>}
        {!listData?.length > 0 && <div className="my-10 text-3xl font-semibold text-center ">No Data</div>}
      </div>
    </AdminLayout>
  )
}
