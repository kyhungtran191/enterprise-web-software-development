import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
import { CustomTable } from '@/components/CustomTable.jsx'
import { ArrowUpDown, Download, ScrollText } from 'lucide-react'
import DynamicBreadcrumb from './DynamicBreadcrumbs'
import { NewUserDialog } from './NewUserDialog'
import { Pencil, UserRoundX, EllipsisVertical, User } from 'lucide-react'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu'
import useParamsVariables from '@/hooks/useParams'
import { useQuery } from '@tanstack/react-query'
import { Contributions } from '@/services/admin'
import { format } from 'date-fns'
import Spinner from './Spinner'
import { isUndefined, omitBy, set } from 'lodash'

import { useContext, useState } from 'react'
import { AbilityContext, Can } from './casl/Can'
import { PERMISSIONS } from '@/constant/casl-permissions'
import JSZip from 'jszip'
import { saveAs } from 'file-saver'
import { ActivityLogDialog } from './ActivityLogDialog'

export function ContributionTable() {
  const [isOpenViewActivityLog, setIsOpenViewActivityLog] = useState(false)
  const [viewContributionLog, setViewContributionLog] = useState(null)
  const handleViewActivityLog = (contribution) => {
    console.log(contribution)

    setIsOpenViewActivityLog(true)
    setViewContributionLog(contribution)
  }
  console.log(viewContributionLog)
  const { data: activityLogs } = useQuery({
    queryKey: ['adminContributionLog', viewContributionLog?.id],
    queryFn: (_) =>
      Contributions.getContributionActivityLogs(viewContributionLog?.id),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000,
    enabled: viewContributionLog != null
  })
  const logs = activityLogs?.data?.responseData || []
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
      accessorKey: 'status',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Status
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
      accessorKey: 'publishDate',
      header: ({ column }) => {
        return (
          <Button
            variant='ghost'
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            Publish Date
            <ArrowUpDown className='w-4 h-4 ml-2' />
          </Button>
        )
      }
    },
    {
      id: 'actions',
      cell: ({ row }) => {
        return (
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <EllipsisVertical className='cursor-pointer' />
            </DropdownMenuTrigger>
            <DropdownMenuContent className='w-56'>
              <DropdownMenuGroup>
                <DropdownMenuItem
                  onSelect={() => downloadContributionFiles(row.original)}
                >
                  <Download className='w-4 h-4 mr-2' />
                  <span>Download contribution</span>
                </DropdownMenuItem>
                <DropdownMenuItem
                  onSelect={() => handleViewActivityLog(row.original)}
                >
                  <ScrollText className='w-4 h-4 mr-2' />
                  <span>Activity logs</span>
                </DropdownMenuItem>
              </DropdownMenuGroup>
            </DropdownMenuContent>
          </DropdownMenu>
        )
      }
    }
  ]
  const [isLoadingFiles, setIsLoadingFiles] = useState(false)

  const queryParams = useParamsVariables()
  const queryConfig = omitBy(
    {
      pageindex: queryParams.pageindex || '1',
      pagesize: queryParams.pagesize || '10'
    },
    isUndefined
  )
  const { data, isLoading } = useQuery({
    queryKey: ['adminContributions', queryParams],
    queryFn: (_) => Contributions.getAllContributionsPaging(queryParams),
    keepPreviousData: true,
    staleTime: 3 * 60 * 1000
  })
  const contributions = data
    ? data?.data?.responseData?.results.map((contribution) => ({
        ...contribution,
        submissionDate: contribution.submissionDate
          ? format(new Date(contribution.submissionDate), 'MM-dd-yyyy')
          : 'Not published',
        publishDate: contribution?.publicDate
          ? format(new Date(contribution.publicDate), 'MM-dd-yyyy')
          : 'Not published'
      }))
    : []
  const [selectedRow, setSelectedRow] = useState({})
  const ability = useContext(AbilityContext)

  async function addFilesToZip(zip, files, folderName) {
    const folder = zip.folder(folderName)
    const fetchPromises = files.map((file) => {
      return fetch(file.path)
        .then((response) => {
          if (response.status === 200) {
            return response.blob()
          }
          throw new Error(`Failed to load the file: ${file.name}`)
        })
        .then((blob) => folder.file(file.name.trim(), blob))
        .catch((error) => {
          console.error(error)
          throw error
        })
    })

    await Promise.all(fetchPromises)
  }
  async function downloadAllContributions() {
    const zip = new JSZip()
    setIsLoadingFiles(true)

    try {
      const addFilePromises = contributions.map((contribution) => {
        const folderName = contribution.title.replace(/[^a-z0-9]/gi, '_')
        return addFilesToZip(zip, contribution.files, folderName)
      })

      await Promise.all(addFilePromises)
      const content = await zip.generateAsync({ type: 'blob' })
      saveAs(content, 'all_contributions.zip')
    } catch (error) {
      alert(
        'Failed to download some files. Please check the console for more details.'
      )
    }

    setIsLoadingFiles(false)
  }

  async function downloadContributionFiles(contribution) {
    setIsLoadingFiles(true)
    const zip = new JSZip()
    const folderName = contribution.title.replace(/[^a-z0-9]/gi, '_')

    try {
      await addFilesToZip(zip, contribution.files, folderName)
      const content = await zip.generateAsync({ type: 'blob' })
      saveAs(content, `${folderName}.zip`)
    } catch (error) {
      alert(
        `Failed to download files for ${folderName}. Please check the console for more details.`
      )
    }

    setIsLoadingFiles(false)
  }

  return (
    <div className='w-full p-4'>
      <div className='flex flex-row justify-between'>
        <DynamicBreadcrumb />
        {/* <Can
          I={PERMISSIONS.Download.Contributions.index}
          a={PERMISSIONS.Download.Contributions.value}
        > */}
        {isLoadingFiles ? (
          <Spinner className={'border-blue-400'}></Spinner>
        ) : (
          <Button onClick={downloadAllContributions}>
            Download all contributions
          </Button>
        )}
        {/* </Can> */}
      </div>
      {isLoading && (
        <div className='container flex items-center justify-center min-h-screen'>
          <Spinner className={'border-blue-500'}></Spinner>
        </div>
      )}
      <ActivityLogDialog
        isOpen={isOpenViewActivityLog}
        handleOpenChange={setIsOpenViewActivityLog}
        logs={logs}
        contribution={viewContributionLog}
      />
      {!isLoading && (
        <div className='h-full px-4 py-6 lg:px-8'>
          <CustomTable
            columns={columns}
            data={contributions}
            path={'/admin/contributions'}
            queryConfig={queryConfig}
            pageCount={data?.data?.responseData.pageCount || 1}
            selectedRows={setSelectedRow}
          />
        </div>
      )}
    </div>
  )
}
