import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import { CustomTable } from '@/components/CustomTable.jsx'
import { ArrowUpDown } from 'lucide-react'
import DynamicBreadcrumb from './DynamicBreadcrumbs'
import useParamsVariables from '@/hooks/useParams'
import { useQuery } from '@tanstack/react-query'
import { Contributions } from '@/services/admin'
import { format } from 'date-fns'
import Spinner from './Spinner'
import { isUndefined, omitBy } from 'lodash'

import { useState } from 'react'

export function NotCommentsContributionTable() {
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
            Title
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },

    {
      accessorKey: 'userName',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Student Name
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'facultyName',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Faculty
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'academicYear',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Academic Year
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'submissionDate',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Submission Date
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      accessorKey: 'isComment',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Comment
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    }
  ]

  const queryParams = useParamsVariables()
  const queryConfig = omitBy(
    {
      pageindex: queryParams.pageindex || '1',
      pagesize: queryParams.pagesize || '10'
    },
    isUndefined
  )
  const { data, isLoading } = useQuery({
    queryKey: ['adminNotCommentContributions', queryConfig],
    queryFn: (_) => Contributions.getNotCommentContributionsPaging(queryConfig),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const contributions = data
    ? data?.data?.responseData?.results.map((contribution) => ({
        ...contribution,
        submissionDate: contribution.submissionDate
          ? format(new Date(contribution.submissionDate), 'MM-dd-yyyy')
          : 'Not published',
        isComment: contribution.isCoordinatorComment
          ? 'Commented'
          : 'Not Commented'
      }))
    : []
  const [selectedRow, setSelectedRow] = useState({})
  console.log(data?.data?.responseData)
  console.log(contributions)
  return (
    <div className='w-full p-4'>
      <div className='flex flex-row justify-between'>
        <DynamicBreadcrumb />
      </div>
      {isLoading && (
        <div className='container flex items-center justify-center min-h-screen'>
          <Spinner className={'border-blue-500'}></Spinner>
        </div>
      )}

      {!isLoading && (
        <div className='h-full px-4 py-6 lg:px-8'>
          <CustomTable
            columns={columns}
            data={contributions}
            path={'/mm/not-comment-contributions'}
            queryConfig={queryConfig}
            pageCount={data?.data?.responseData.pageCount || 1}
            selectedRows={setSelectedRow}
          />
        </div>
      )}
    </div>
  )
}
