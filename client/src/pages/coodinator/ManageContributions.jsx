import Search from '@/components/Search'
import { Button } from '@/components/ui/button'
import AdminLayout from '@/layouts/AdminLayout'
import { ArrowDown10Icon, Plus } from 'lucide-react'
import React, { useCallback, useEffect, useRef, useState } from 'react'
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
import { useQuery, useQueryClient } from '@tanstack/react-query'
import useParamsVariables from '@/hooks/useParams'
import { isUndefined, omitBy, omit, debounce } from 'lodash'
import { Icon } from '@iconify/react'
import PaginationCustom from '@/components/PaginationCustom'
import { Checkbox } from "@/components/ui/checkbox"
import { ArrowUpDown } from 'lucide-react'

import Spinner from '@/components/Spinner'
import { MC_OPTIONS, STUDENT_OPTIONS } from '@/constant/menuSidebar'
import { Contributions } from '@/services/coodinator'
import { CustomTable } from '@/components/CustomTable'
import { MC_Tables_Columns } from '@/constant/table-columns'
import { formatDate } from '@/utils/helper'
export default function ManageContribution() {
  const [position, setPosition] = React.useState('')
  const navigate = useNavigate()
  const queryParams = useParamsVariables()
  const [inputValue, setInputValue] = useState('');
  const columns = [
    {
      id: 'select',
      header: ({ table }) => (
        <Checkbox
          checked={
            table.getIsAllPageRowsSelected() ||
            (table.getIsSomePageRowsSelected() && 'indeterminate')
          }
          onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
          aria-label='Select all'
          className='mx-4'
        />
      ),
      cell: ({ row }) => (
        <Checkbox
          checked={row.getIsSelected()}
          onCheckedChange={(value) => row.toggleSelected(!!value)}
          aria-label='Select row'
        />
      ),
      enableSorting: false,
      enableHiding: false
    },
    {
      accessorKey: 'title',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Contribution Title
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'shortDescription',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Short Description
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },

    {
      accessorKey: `${formatDate('publicDate')}`,
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Date
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'files.length',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Files
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
  ]
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
    queryKey: ['mc-contributions', queryConfig],
    queryFn: (_) => Contributions.MCContribution(queryConfig)
  })

  const handleNavigateStatus = (status) => {
    setPosition(status);
    if (status !== undefined) {
      navigate({
        pathname: "/coodinator-manage/contributions",
        search: createSearchParams(
          omitBy(
            {
              ...queryConfig,
              status: status
            },
            (value, key) => key === 'pageindex' || key === 'pagesize' || isUndefined(value)
          )
        ).toString()
      });
    } else {
      return navigate({
        pathname: "/coodinator-manage/contributions",
        search: createSearchParams(omit({ ...queryConfig }, ['status'])).toString()
      });
    }
  }

  useEffect(() => {
    if (queryParams['status']) {
      setPosition(queryParams['status'])
    }
    inputRef.current.focus()
  }, [queryParams]);

  const inputRef = useRef(null)
  const handleInputChange = useCallback(
    debounce((value) => {
      if (!value) {
        return navigate({
          pathname: "/coodinator-manage/contributions",
          search: createSearchParams(omit({ ...queryConfig }, ['keyword'])).toString()
        });
      }
      navigate({
        pathname: "/coodinator-manage/contributions",
        search: createSearchParams(omitBy({
          ...queryConfig,
          keyword: value
        }, (value, key) => key === 'pageindex' || key === 'pagesize' || isUndefined(value))).toString()
      });
    }, 300),
    [navigate]
  );

  const currentData = data && data?.data?.responseData;
  return (
    <AdminLayout links={MC_OPTIONS}>
      <div className='flex flex-wrap items-center gap-3 my-5'>
        <div className={`flex items-center px-5 py-4 border rounded-lg gap-x-2 w-[50vw]`}>
          <Icon icon="ic:outline-search" className="flex-shrink-0 w-6 h-6 text-slate-700"></Icon>
          <input type="text" className='flex-1 border-none outline-none' placeholder="What you're looking for ?"
            ref={inputRef}
            defaultValue={queryParams["keyword"]}
            onChange={(e) => {
              setInputValue(e.target.value)
              handleInputChange(e.target.value);
            }}
          />
        </div>
        <div className='flex flex-wrap items-center gap-2'>
          <div className='flex-1'>
            <DropdownMenu>
              <DropdownMenuTrigger asChild className='w-full'>
                <Button
                  variant='default'
                  className='gap-4 border-none outline-none py-7 min-w-[145px]'
                >
                  {position?.toUpperCase() || "Filter Status"} <ArrowDown10Icon></ArrowDown10Icon>
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent className='w-56'>
                <DropdownMenuRadioGroup
                  value={position}
                >
                  {position != "" && <DropdownMenuRadioItem value='' onClick={() => handleNavigateStatus(undefined)}>
                    All
                  </DropdownMenuRadioItem>}
                  <DropdownMenuRadioItem value='PENDING' onClick={() => handleNavigateStatus("PENDING")}>
                    Pending
                  </DropdownMenuRadioItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuRadioItem value='APPROVE' onClick={() => handleNavigateStatus("APPROVE")} >
                    Approve
                  </DropdownMenuRadioItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuRadioItem value='REJECT' onClick={() => handleNavigateStatus("REJECT")}>
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
        <PaginationCustom path={"/coodinator-manage/contributions"} queryConfig={queryConfig} totalPage={data?.data?.responseData.pageCount || 1}></PaginationCustom>
      </>}
      {isLoading && <div className="flex justify-center min-h-screen mt-10">
        <Spinner></Spinner>
      </div>}
      {!isLoading && !currentData?.results?.length > 0 && <div className="my-10 text-3xl font-semibold text-center ">No Data</div>}
    </AdminLayout>
  )
}
