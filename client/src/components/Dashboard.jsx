import React, { useState } from 'react'

import DynamicBreadcrumb from './DynamicBreadcrumbs'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue
} from '@/components/ui/select'
import BarChart from './BarChart'

const Dashboard = () => {
  const [isStacked, setIsStacked] = useState(false)
  const options = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top'
      },
      title: {
        display: true,
        text: 'Number of contributions within each Faculty for each academic year'
      }
    }
  }
  const labels = [
    '2021-2022',
    '2022-2023',
    '2023-2024',
    '2024-2025',
    '2025-2026',
    '2026-2027'
  ]

  const data = {
    labels,
    datasets: [
      {
        label: 'IT',
        data: labels.map(() => Math.random(1000)),
        backgroundColor: '#86efac'
      },
      {
        label: 'Design',
        data: labels.map(() => Math.random(1000)),
        backgroundColor: '#7dd3fc'
      },
      {
        label: 'Business',
        data: labels.map(() => Math.random(1000)),
        backgroundColor: '#fda4af'
      }
    ]
  }
  return (
    <div className='w-full p-4'>
      <div className='flex flex-row justify-between'>
        <DynamicBreadcrumb />
        {/* <NewRoleDialog /> */}
      </div>
      <div className='flex flex-row space-x-2 my-4'>
        <Select>
          <SelectTrigger className='w-[280px]'>
            <SelectValue placeholder='Select charts' />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value='light'>Number of contributions</SelectItem>
            <SelectItem value='dark'>Percentage of contributions</SelectItem>
            <SelectItem value='system'>Number of contributors</SelectItem>
          </SelectContent>
        </Select>
        <Select>
          <SelectTrigger className='w-[180px]'>
            <SelectValue placeholder='Academic Year' />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value='light'>2021-2022</SelectItem>
            <SelectItem value='dark'>2022-2023</SelectItem>
            <SelectItem value='system'>2023-2024</SelectItem>
          </SelectContent>
        </Select>
      </div>
      {/* {isLoadingRoles && (
    <div className='container flex items-center justify-center min-h-screen'>
      <Spinner className={'border-blue-500'}></Spinner>
    </div>
  )} */}
      <div className='h-full px-4 py-6 lg:px-8'>
        <BarChart data={data} options={options} />
      </div>
    </div>
  )
}
export default Dashboard
