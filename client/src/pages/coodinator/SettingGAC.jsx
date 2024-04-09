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
import { formatDate } from '@/utils/helper'



import {
  getCoreRowModel,
  getFilteredRowModel,
  getPaginationRowModel,
  useReactTable,
} from '@tanstack/react-table'


export default function SettingGAC() {
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
  const [rowSelection, setRowSelection] = React.useState({})

  const queryConfig = omitBy(
    {
      pageindex: queryParams.pageindex || '1',
      facultyname: queryParams.facultyname,
      status: 'APPROVE',
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


  useEffect(() => {
    if (queryParams['status']) {
      setPosition(queryParams['status'])
    }
  }, [queryParams]);

  const handleInputChange = debounce((value) => {
    if (!value) {
      return navigate({
        pathname: "/coodinator-manage/setting-guest",
        search: createSearchParams(omit({ ...queryConfig }, ['keyword'])).toString()
      });
    }

    navigate({
      pathname: "/coodinator-manage/setting-guest",
      search: createSearchParams(omitBy({
        ...queryConfig,
        keyword: value
      }, (value, key) => key === 'pageindex' || key === 'pagesize' || isUndefined(value))).toString()
    });
  }, 300);

  const currentData = data && data?.data?.responseData;
  const table = useReactTable({
    data: currentData?.results,
    columns,
    state: {
      rowSelection,
    },
    enableRowSelection: true, //enable row selection for all rows
    // enableRowSelection: row => row.original.age > 18, // or enable row selection conditionally per row
    onRowSelectionChange: setRowSelection,
    getCoreRowModel: getCoreRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    debugTable: true,
  })
  // console.log(rowSelection)
  // console.log(Object.keys(rowSelection).length)
  return (
    <AdminLayout links={MC_OPTIONS}>
      <div className='flex flex-wrap items-center gap-3 my-5'>
        <div className={`flex items-center px-5 py-4 border rounded-lg gap-x-2 w-[50vw]`}>
          <Icon icon="ic:outline-search" className="flex-shrink-0 w-6 h-6 text-slate-700"></Icon>
          <input type="text" className='flex-1 border-none outline-none' placeholder="What you're looking for ?"
            defaultValue={queryParams["keyword"]}
            onChange={(e) => {
              setInputValue(e.target.value)
              handleInputChange(e.target.value);
            }}
          />
        </div>
        {Object.keys(rowSelection)?.length > 0 && <Button>Add selection</Button>}
      </div>
      {
        !isLoading && <>
          <CustomTable columns={columns} path={'/coodinator-manage/setting-guest'} queryConfig={queryConfig} pageCount={currentData?.pageCount} table={table}></CustomTable>
        </>

      }
      {
        isLoading && <div className="flex justify-center min-h-screen mt-10">
          <Spinner></Spinner>
        </div>
      }

      {!isLoading && currentData?.results?.length < 0 && <div className="my-10 text-3xl font-semibold text-center ">No Data</div>}
    </AdminLayout >
  )
}
